using Logging_lib.Model;
using Messaging_lib.Messaging;
using Messaging_lib.Stream;

namespace Logging_lib.Logging;

public class ElasticLogger : IAsyncDisposable
{
    private static List<ElasticLogger> _instances = new ();

    private static string _url;

    private static string _appName;

    private string _loggerName;
    
    private DotPulsarProducer? _logProducer;
    
    public bool InError { get; private set; }

    public string ErrorMessage { get; private set; } = "";

    public ElasticLogger(string loggerName)
    {
        _loggerName = loggerName;
        _instances.Add(this);
    }
    
    public async Task Debug(String message) {
        await Produce(new LogMessage
        {
            ClassName = _loggerName,
            Level = LogLevel.Debug,
            Timestamp = DateTime.UtcNow,
            Message = message
        });
    }
    
    public async Task Info(String message) {
        await Produce(new LogMessage
        {
            ClassName = _loggerName,
            Level = LogLevel.Info,
            Timestamp = DateTime.UtcNow,
            Message = message
        });
    }
    
    public async Task Warn(String message) {
        await Produce(new LogMessage
        {
            ClassName = _loggerName,
            Level = LogLevel.Warning,
            Timestamp = DateTime.UtcNow,
            Message = message
        });
    }
    
    public async Task Error(String message) {
        await Produce(new LogMessage
        {
            ClassName = _loggerName,
            Level = LogLevel.Error,
            Timestamp = DateTime.UtcNow,
            Message = message
        });
    }

    private async Task Produce(LogMessage message)
    {
        if (_logProducer == null || _logProducer.InError)
        {
            if (_logProducer != null)
            {
                await _logProducer.DisposeAsync();
            }
            _logProducer = CreateLoggerProducer("logs");
        }
        var stream = new MemoryOutputStream();
        message.WriteTo(stream);
        await _logProducer.Send(stream.GetBuffer());
    }

    public async ValueTask DisposeAsync()
    {
        if(_logProducer != null) await _logProducer.DisposeAsync();
    }

    public static void Init(string url, string appName)
    {
        _url = url;
        _appName = appName;
    }

    public static async Task Shutdown()
    {
        foreach (var logger in _instances)
        {
            await logger.DisposeAsync();
        }
    }

    private static DotPulsarProducer CreateLoggerProducer(string topicName)
    {
        return new DotPulsarProducer(_url, $"{_appName}-tn", $"{_appName}-ns", topicName, true);
    }
}