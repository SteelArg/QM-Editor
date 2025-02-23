using System;

namespace QMEditor;

public class PartialLoggerServiceProvider : ILoggerService {
    
    private readonly ILoggerService _loggerService;
    private readonly LogLevel _logLevel;

    public PartialLoggerServiceProvider(ILoggerService loggerService, LogLevel logLevel) {
        _loggerService = loggerService;
        _logLevel = logLevel;
    }

    public void Log(string message) {
        if (!_logLevel.HasFlag(LogLevel.Logs)) return;
        _loggerService.Log(message);
    }

    public void Warning(string message) {
        if (!_logLevel.HasFlag(LogLevel.Warnings)) return;
        _loggerService.Warning(message);
    }

    public void Error(string message) {
        if (!_logLevel.HasFlag(LogLevel.Errors)) return;
        _loggerService.Error(message);
    }

}

[Flags]
public enum LogLevel {
    None = 0,
    Logs = 1,
    Warnings = 2,
    Errors = 4,
    All = Logs | Warnings | Errors,
    Debug = All,
    Release = Errors
}