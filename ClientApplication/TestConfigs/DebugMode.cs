using System;
using System.Linq;

namespace ClientApplication.TestConfigs;
/// <summary>
///  Diese Klasse steuert die Ausführung der Anwendung im Debug- bzw. Normalmodus
/// </summary>
public static class DebugMode
{
    #if DEBUG
        private static readonly bool IsDebugMode = true;
    #else
        private static readonly bool IsDebugMode = false;
    #endif
    private static readonly Random Random = new();

    private static bool IsInDebugMode()
    {
        if (IsDebugMode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static string? GetDebugModeIpAddress()
    {
        if (IsInDebugMode())
        {
            return "localhost";
        }
        return null;
    }
    
    public static int GetDebugModePort()
    {
        if (IsInDebugMode())
        {
            return 8000;
        }
        return -1;
    }
    
    public static string? GetDebugModeTestName()
    {
        if (IsInDebugMode())
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        return null;
    }
}