using System.Collections.Concurrent;
using System.Text;

namespace Exchange.Infrastructure;

public static class Logger
{
    private static readonly BlockingCollection<string> _logQueue = new();
    private static readonly Task _logWriterTask;
    private static readonly string _logFilePath;

    static Logger()
    {
        var logDir = Path.Combine(AppContext.BaseDirectory, "logs");
        Directory.CreateDirectory(logDir);
        _logFilePath = Path.Combine(logDir, $"log_{DateTime.Now:yyyyMMdd_HHmmss}.log");

        _logWriterTask = Task.Factory.StartNew(() =>
        {
            using var writer = new StreamWriter(_logFilePath, append: true, Encoding.UTF8);
            foreach (var message in _logQueue.GetConsumingEnumerable())
            {
                writer.WriteLine(message);
                writer.Flush();
            }
        }, TaskCreationOptions.LongRunning);
    }

    public static void Info(string message) => Log("INFO", message);
    public static void Warn(string message) => Log("WARN", message);
    public static void Error(string message) => Log("ERROR", message);

    private static void Log(string level, string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        _logQueue.Add($"[{timestamp}] [{level}] {message}");
    }

    public static void Shutdown() => _logQueue.CompleteAdding();
}
