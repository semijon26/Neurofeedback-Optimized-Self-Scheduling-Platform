using System;
using System.Windows.Input;
using ClientApplication.Models;
using Serilog;
using Shared;
using WebSocketSharp;

/// <summary>
///  Statische Klasse, die die Methoden zum Logging bereitstellt.
/// </summary>

namespace ClientApplication.Utils
{
    public static class Logging
    {
        // Logfile für clientseitige Logs (Für Debugging)
        static string logFileName = $"logs_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        
        private static readonly ILogger logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logFileName)
                .WriteTo.Debug()
                .CreateLogger();
        
        private static WebSocket? _loggingWebSocket;

        // Event-Logs sollen an Server geschickt werden, daher wird hier eine Websocket Verbindung aufgebaut
        public static void InitServerLogging(string ip, int port)
        {
            try
            {
                _loggingWebSocket = new WebSocket($"ws://{ip}:{port}/logging");
                _loggingWebSocket.Connect();
            }
            catch (Exception ex)
            {
                LogError(message: "Init Logging Websocket failed", ex: ex);
            }
        }

        // Logging von UserInteraction, die an Server übertragen werden
        public static void LogUserInteraction(string msg)
        {
            _InternalLogEvent(new EventLogEntry{User = GetClientInstanceLogging(), Message = msg, Type = EventLogType.USER_INTERACTION});
        }
        
        // Logging von GameEvents, die an Server übertragen werden
        public static void LogGameEvent(string msg)
        {
            _InternalLogEvent(new EventLogEntry{User = GetClientInstanceLogging(), Message = msg, Type = EventLogType.GAME_EVENT});
        }
        
        // Logging von NeuroFeedbackEvents, die an Server übertragen werden
        public static void LogNeuroEvent(string msg)
        {
            _InternalLogEvent(new EventLogEntry{Message = msg, Type = EventLogType.NEURO_EVENT});
        }

        // Logging von KeyEvents, die an Server übertragen werden
        public static void LogKeyEvent(Key key)
        {
            KeyConverter converter = new KeyConverter();
            _InternalLogEvent(new EventLogEntry { Message = converter.ConvertToString(key) ?? "unknown key", Type = EventLogType.KEY_EVENT, User = GetClientInstanceLogging()});
        }
        
        private static void _InternalLogEvent(EventLogEntry e) {
            try
            {
                var byteData = SocketMessageHelper.SerializeToByteArray(e);
                if (_loggingWebSocket != null)
                {
                    _loggingWebSocket.Send(byteData);
                }
                else
                {
                    LogError("Logging of event failed: LoggingWebSocket not init");
                }
                
            }
            catch (Exception exception)
            {
                LogError("Logging of event failed", exception);
            }
        }

    // Folgend alle Log-Möglichkeite, die in die log-File der ClientAnwendung loggen
        public static void LogInformation(string message)
        {
            logger.Information(message);
        }

        public static void LogError(string message, Exception ex)
        {
            logger.Error(ex, message);
        }

        public static void LogError(string message)
        {
            logger.Error(message);
        }

        public static void LogError(Exception ex)
        {
            logger.Error(ex, "");
        }

        public static void LogWarning(string message)
        {
            logger.Warning(message);
        }
        
        // Für Serverlogs den aktuellen Client extrahieren, um den Logeintrag richtig zuzuordnen.
        private static string GetClientInstanceLogging()
        {
            var currentClient = ClientManagementData.GetInstance(ClientObject.GetInstance()).CurrentClient.Label;
            return currentClient;
        }
    }
}
