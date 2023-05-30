using System.Globalization;
using Serilog;
using Shared;
using WebSocketSharp;

namespace ServerApplication.modules;

public static class Logging
{
    static string logDate = $"{DateTime.Now:yyyyMMdd_HHmmss}";

    private static readonly ILogger ApplicationLogger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File($"log_{logDate}.txt")
        .CreateLogger();

    private const string RawMessagWithNewlineTemplate = "{Message:lj}{NewLine}";

    private const string CsvSeparator = ";";

    private static readonly ILogger EventLogger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console(outputTemplate: RawMessagWithNewlineTemplate)
        .WriteTo.File($"event_log_{logDate}.csv", outputTemplate: RawMessagWithNewlineTemplate)
        .CreateLogger();

    static Logging()
    {
        // first line in csv should be the header
        EventLogger.Information($"timestamp;user;event_type;message");
    }

    public static void LogEvent(EventLogEntry logEntry)
    {
        if (logEntry.Message.Contains(CsvSeparator) || logEntry.Message.Contains('\n') || logEntry.Message.Contains("\r\n"))
        {
            throw new ArgumentException($"Log message must not contain \"{CsvSeparator}\" and newlines!");
        }

        var timestamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
        EventLogger.Information($"{timestamp}{CsvSeparator}{logEntry.User}{CsvSeparator}{logEntry.Type}{CsvSeparator}{logEntry.Message}");
    }

    public static void LogInformation(string message)
    {
        ApplicationLogger.Information(message);
    }

    public static void LogError(string message)
    {
        ApplicationLogger.Error(message);
    }

    public static void LogWarning(string message)
    {
        ApplicationLogger.Warning(message);
    }
}