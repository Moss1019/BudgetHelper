using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;

namespace Messaging_lib.Messaging;

public class DotPulsarConsumer : IAsyncDisposable
{
    private readonly CancellationTokenSource _cts = new();
    
    private Task? _work;

    private IPulsarClient _client;

    private IConsumer<byte[]> _consumer;

    private Func<byte[], CancellationToken, Task> _callback;
    
    public bool InError { get; private set; } = false;

    public string Error { get; private set; } = "";
    
    public DotPulsarConsumer(string pulsarUrl, string tenant, string nameSpace, string topic, bool isPersistent, Func<byte[], CancellationToken, Task> callback)
    {
        _callback = callback;
        try
        {
            var uri = new Uri(pulsarUrl);
            _client = PulsarClient
                .Builder()
                .ServiceUrl(uri)
                .Build();

            var topicPath = string.Format("{0}://{1}/{2}/{3}",
                isPersistent ? "persistent" : "non-persistent",
                tenant,
                nameSpace,
                topic);
            _consumer = _client
                .NewConsumer(Schema.ByteArray)
                .Topic(topicPath)
                .SubscriptionName("BudgetApp_Sub")
                .Create();
        }
        catch (Exception ex)
        {
            InError = true;
            Error = ex.Message;
        }
    }

    public async Task<byte[]> Receive()
    {
        var message = await _consumer.Receive();
        await  _consumer.Acknowledge(message, CancellationToken.None);
        return message.Value();
    }
    
    public void Start() => _work = Task.Run(() => RunAsync(_cts.Token));

    public async Task Stop()
    {
        _cts.Cancel();
        if (_work != null) await _work;
    }
    
    public async ValueTask DisposeAsync()
    {
        await Stop();
        await _client.DisposeAsync();
        await _consumer.DisposeAsync();
    }

    private async Task RunAsync(CancellationToken ct)
    {
        if (InError) return;
        try
        {
            await foreach (var message in _consumer.Messages(ct))
            {
                try
                {
                    var buffer = message.Value();
                    await _callback(buffer, ct);
                    await _consumer.Acknowledge(message, ct);
                }
                catch (Exception e)
                {
                    // TODO: Handle error
                }
            }
        }
        catch (OperationCanceledException)
        {
            
        }
    }
}