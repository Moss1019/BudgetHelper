using Logging_lib.Logging;

namespace Logging_libTest.Logging;

public class LoggerTest
{
    [Test]
    public async Task TestLogInfo_ShowsUpInPulsar()
    {
        ElasticLogger.Init("pulsar://localhost:6650", "budget");
        
        var sut = new ElasticLogger("test-logger");
        await sut.Debug("This is a debug message");
        await sut.Info("This is a info message");
        await sut.Warn("This is a warn message");
        await sut.Error("This is a error message");

        await ElasticLogger.Shutdown();
    }
}