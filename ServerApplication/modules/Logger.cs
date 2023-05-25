using Serilog;
using Shared;

namespace ServerApplication.modules;

public static class Logging
{

    private static readonly ILogger logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs.txt")
                .CreateLogger();

    private static readonly string _eventLogOutputTemplate = "[{Timestamp:HH:mm:ss}] {User:} {Message:lj}{NewLine}{Exception}";
    
    private static readonly ILogger EventLogger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console(outputTemplate: _eventLogOutputTemplate)
        .WriteTo.File("event_logs.txt", outputTemplate: _eventLogOutputTemplate)
        .CreateLogger();

    public static void LogEvent(EventLogEntry logEntry)
    {
        EventLogger.Information($"User:{logEntry.User} {logEntry.Type}: {logEntry.Message}");
    }

    public static void LogInformation(string message)
    {
        logger.Information(message);
    }

    public static void LogError(string message)
    {
        logger.Error(message);
    }

    public static void LogWarning(string message)
    {
        logger.Warning(message);
    }
}
