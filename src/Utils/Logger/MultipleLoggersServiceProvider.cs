namespace QMEditor;

public class MultipleLoggersServiceProvider : ILoggerService {

    private readonly ILoggerService[] _loggerServices;

    public MultipleLoggersServiceProvider(ILoggerService[] loggerServices) {
        _loggerServices = loggerServices;
    }

    public void Log(string message) {
        foreach (ILoggerService logger in _loggerServices) {
            logger.Log(message);
        }
    }

    public void Warning(string message) {
        foreach (ILoggerService logger in _loggerServices) {
            logger.Warning(message);
        }
    }

    public void Error(string message) {
        foreach (ILoggerService logger in _loggerServices) {
            logger.Error(message);
        }
    }

}