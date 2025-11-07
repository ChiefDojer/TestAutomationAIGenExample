using System.Collections.Concurrent;

namespace GitHubCopilotDocs.Selenium.Tests.Core.Logging;

/// <summary>
/// Thread-safe logger for test execution with structured output.
/// </summary>
public sealed class TestLogger
{
    private readonly string _logFilePath;
    private readonly LogLevel _minimumLevel;
    private readonly bool _consoleEnabled;
    private readonly bool _fileEnabled;
    private readonly ConcurrentQueue<string> _logQueue = new();

    /// <summary>
    /// Log levels in increasing severity.
    /// </summary>
    public enum LogLevel
    {
        Trace = 0,
        Debug = 1,
        Information = 2,
        Warning = 3,
        Error = 4,
        Critical = 5
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestLogger"/> class.
    /// </summary>
    /// <param name="testName">Name of the test for log file naming.</param>
    /// <param name="minimumLevel">Minimum log level to capture.</param>
    /// <param name="outputPath">Directory path for log files.</param>
    /// <param name="consoleEnabled">Whether to output to console.</param>
    /// <param name="fileEnabled">Whether to output to file.</param>
    public TestLogger(string testName, LogLevel minimumLevel, string outputPath, bool consoleEnabled, bool fileEnabled)
    {
        _minimumLevel = minimumLevel;
        _consoleEnabled = consoleEnabled;
        _fileEnabled = fileEnabled;

        if (_fileEnabled)
        {
            Directory.CreateDirectory(outputPath);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            _logFilePath = Path.Combine(outputPath, $"{testName}_{timestamp}.log");
        }
        else
        {
            _logFilePath = string.Empty;
        }
    }

    /// <summary>
    /// Logs a trace-level message.
    /// </summary>
    public void Trace(string message) => Log(LogLevel.Trace, message);

    /// <summary>
    /// Logs a debug-level message.
    /// </summary>
    public void Debug(string message) => Log(LogLevel.Debug, message);

    /// <summary>
    /// Logs an information-level message.
    /// </summary>
    public void Information(string message) => Log(LogLevel.Information, message);

    /// <summary>
    /// Logs a warning-level message.
    /// </summary>
    public void Warning(string message) => Log(LogLevel.Warning, message);

    /// <summary>
    /// Logs an error-level message with optional exception.
    /// </summary>
    public void Error(string message, Exception? exception = null)
    {
        var fullMessage = exception != null ? $"{message}\n{exception}" : message;
        Log(LogLevel.Error, fullMessage);
    }

    /// <summary>
    /// Logs a critical-level message with optional exception.
    /// </summary>
    public void Critical(string message, Exception? exception = null)
    {
        var fullMessage = exception != null ? $"{message}\n{exception}" : message;
        Log(LogLevel.Critical, fullMessage);
    }

    private void Log(LogLevel level, string message)
    {
        if (level < _minimumLevel)
            return;

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var threadId = Environment.CurrentManagedThreadId;
        var logEntry = $"[{timestamp}] [{level,-11}] [Thread-{threadId}] {message}";

        _logQueue.Enqueue(logEntry);

        if (_consoleEnabled)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = GetConsoleColor(level);
            Console.WriteLine(logEntry);
            Console.ForegroundColor = originalColor;
        }

        if (_fileEnabled && !string.IsNullOrEmpty(_logFilePath))
        {
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
    }

    private static ConsoleColor GetConsoleColor(LogLevel level) => level switch
    {
        LogLevel.Trace => ConsoleColor.Gray,
        LogLevel.Debug => ConsoleColor.Cyan,
        LogLevel.Information => ConsoleColor.White,
        LogLevel.Warning => ConsoleColor.Yellow,
        LogLevel.Error => ConsoleColor.Red,
        LogLevel.Critical => ConsoleColor.Magenta,
        _ => ConsoleColor.White
    };

    /// <summary>
    /// Gets the path to the log file if file logging is enabled.
    /// </summary>
    public string LogFilePath => _logFilePath;
}
