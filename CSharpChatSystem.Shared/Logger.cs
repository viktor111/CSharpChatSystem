using System.Text.Json;

namespace CSharpChatSystem.Shared;

public static class Logger
{
    private static readonly object LockObject = new object();
    public static void LogMessage(string message, object obj = null)
    {
        WriteLog("MESSAGE", message, obj);
    }

    public static void LogInformation(string message, object obj = null)
    {
        WriteLog("INFO", message, obj);
    }
    
    public static void LogError(string message, object obj = null)
    {
        WriteLog("ERROR", message, obj);
    }
    
    public static void LogWarning(string message, object obj = null)
    {
        WriteLog("WARNING", message, obj);
    }
    
    public static void LogDebug(string message, object obj = null)
    {
        WriteLog("DEBUG", message, obj);
    }

    private static void WriteLog(string level, string message, object obj)
    {
        lock (LockObject)
        {
            var logMessage = $"{DateTime.Now} [{level}] {message}";
            if (obj != null)
            {
                string serializedObj = JsonSerializer.Serialize(obj);
                logMessage += $" - {serializedObj}";
            }
            Console.WriteLine(logMessage);
        }
    }
}
