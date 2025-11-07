using System.Collections.Concurrent;

namespace GitHubCopilotDocs.Tests.Core.Logging;

/// <summary>
/// Thread-safe structured logger for test execution.
/// Outputs to console and optional file with different log levels.
/// </summary>
public sealed class TestLogger
{
    private readonly string _testName;
    private readonly string _outputPath;
    private readonly LogLevel _minimumLevel;
    private readonly bool _consoleEnabled;
    private readonly bool _fileEnabled;
    private readonly ConcurrentQueue<string> _logBuffer = new();

    public TestLogger(
        string testName,
        LogLevel minimumLevel = LogLevel.Information,
        string outputPath = "TestResults/Logs",
        bool consoleEnabled = true,
        bool fileEnabled = true)
    {
        _testName = testName;
        _minimumLevel = minimumLevel;
        _outputPath = outputPath;
        _consoleEnabled = consoleEnabled;
        _fileEnabled = fileEnabled;

        if (_fileEnabled)
        {
            Directory.CreateDirectory(_outputPath);
        }
    }

    public void Trace(string message) => Log(LogLevel.Trace, message);
    public void Debug(string message) => Log(LogLevel.Debug, message);
    public void Information(string message) => Log(LogLevel.Information, message);
    public void Warning(string message) => Log(LogLevel.Warning, message);
    public void Error(string message, Exception? exception = null)
    {
        var fullMessage = exception != null
            ? $"{message}{Environment.NewLine}{exception}"
            : message;
        Log(LogLevel.Error, fullMessage);
    }

    public void Critical(string message, Exception? exception = null)
    {
        var fullMessage = exception != null
            ? $"{message}{Environment.NewLine}{exception}"
            : message;
        Log(LogLevel.Critical, fullMessage);
    }

    private void Log(LogLevel level, string message)
    {
        if (level < _minimumLevel)
            return;

        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var threadId = Environment.CurrentManagedThreadId;
        var logEntry = $"[{timestamp}] [{level,-11}] [Thread-{threadId}] [{_testName}] {message}";

        _logBuffer.Enqueue(logEntry);

        if (_consoleEnabled)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = GetColorForLevel(level);
            Console.WriteLine(logEntry);
            Console.ForegroundColor = originalColor;
        }

        if (_fileEnabled)
        {
            var fileName = $"{_testName}_{DateTime.UtcNow:yyyyMMdd}.log";
            var filePath = Path.Combine(_outputPath, fileName);

            try
            {
                File.AppendAllText(filePath, logEntry + Environment.NewLine);
            }
            catch (IOException)
            {
                // Swallow file write errors to prevent test failures
            }
        }
    }

    private static ConsoleColor GetColorForLevel(LogLevel level) => level switch
    {
        LogLevel.Trace => ConsoleColor.Gray,
        LogLevel.Debug => ConsoleColor.DarkGray,
        LogLevel.Information => ConsoleColor.White,
        LogLevel.Warning => ConsoleColor.Yellow,
        LogLevel.Error => ConsoleColor.Red,
        LogLevel.Critical => ConsoleColor.DarkRed,
        _ => ConsoleColor.White
    };

    public IReadOnlyCollection<string> GetLogBuffer() => _logBuffer.ToArray();
}

public enum LogLevel
{
    Trace = 0,
    Debug = 1,
    Information = 2,
    Warning = 3,
    Error = 4,
    Critical = 5
}
