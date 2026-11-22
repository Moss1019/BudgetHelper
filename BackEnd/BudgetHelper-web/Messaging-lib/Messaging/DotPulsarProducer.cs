using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using DotPulsar.Internal;

namespace Messaging_lib.Messaging;

public class DotPulsarProducer : IAsyncDisposable
{
    private IPulsarClient _client;

    private IProducer<byte[]> _producer;
    
    public bool InError { get; private set; } = false;

    public string Error { get; private set; } = "";

    public DotPulsarProducer(string pulsarUrl, string tenant, string nameSpace, string topic, bool isPersistent)
    {
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
            _producer = _client
                .NewProducer(Schema.ByteArray)
                .Topic(topicPath)
                .Create();
        }
        catch (Exception ex)
        {
            InError = true;
            Error = ex.Message;
        }
    }

    public async Task<bool> Send(byte[] message)
    {
        if (InError)
        {
            return false;
        }
        try
        {
            var messageId = await _producer.NewMessage()
                .Send(message);
            return true;
        }
        catch (Exception ex)
        {
            InError = true;
            Error = ex.Message;
            return false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
        await _producer.DisposeAsync();
    }
}