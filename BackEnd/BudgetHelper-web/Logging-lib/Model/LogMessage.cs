using Messaging_lib.Stream;

namespace Logging_lib.Model;

public class LogMessage
{
    public string AppName { get; set; }
    
    public string ClassName { get; set; }
    
    public string Message { get; set; }
    
    public LogLevel Level { get; set; }
    
    public DateTime Timestamp { get; set; }

    public static LogMessage FromInputStream(MemoryInputStream stream)
    {
        var message = new LogMessage();
        message.ReadFrom(stream);
        return message;
    }
}

public static class LogMessageExtensions
{
    public static void WriteTo(this LogMessage message, MemoryOutputStream stream)
    {
        stream.Write(message.AppName);
        stream.Write(message.ClassName);
        stream.Write(message.Timestamp);
        stream.Write(message.Message);
        stream.Write(ConvertLogLevel(message.Level));
    }

    public static void ReadFrom(this LogMessage message, MemoryInputStream stream)
    {
        message.AppName = stream.ReadString();
        message.ClassName = stream.ReadString();
        message.Timestamp = stream.ReadDateTime();
        message.Message = stream.ReadString();
        message.Level = ConvertLogLevelStr(stream.ReadString());
    }

    private static string ConvertLogLevel(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Debug: return "debug";
            case LogLevel.Error: return "error";
            case LogLevel.Info: return "info";
            case LogLevel.Warning: return "warning";
        }
        
        return "info";
    }
    
    private static LogLevel ConvertLogLevelStr(string logLevelStr)
    {
        switch (logLevelStr)
        {
            case "debug": return LogLevel.Debug;
            case "error": return LogLevel.Error;
            case "info": return LogLevel.Info;
            case "warning": return LogLevel.Warning;
        }
        
        return LogLevel.Info;
    }
    
    
}