using System.Globalization;
using Serilog;
using Shared;
using WebSocketSharp;

namespace ServerApplication.modules;

// Die Haupt-Logging Klasse des Servers: Nimmt auch Client-Logs an und schreibt sie in den Event-Log
public static class Logging
{
    static string logDate = $"{DateTime.Now:yyyyMMdd_HHmmss}";

    private static readonly ILogger ApplicationLogger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File($"log_{logDate}.txt")
        .CreateLogger();

    private const string RawMessageWithNewlineTemplate = "{Message:lj}{NewLine}";

    private const string CsvSeparator = ";";

    private static readonly ILogger EventLogger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console(outputTemplate: RawMessageWithNewlineTemplate)
        .WriteTo.File($"event_log_{logDate}.csv", outputTemplate: RawMessageWithNewlineTemplate)
        .CreateLogger();

    static Logging()
    {
        // first line in csv should be the header with column names
        EventLogger.Information($"timestamp{CsvSeparator}user{CsvSeparator}event_type{CsvSeparator}message");
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

    
    // Diese Funktionen sind für Server-Debug-Zwecke
    
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