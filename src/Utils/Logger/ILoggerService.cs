namespace QMEditor;

public interface ILoggerService {

    public void Log(string message);

    public void Warning(string message);

    public void Error(string message);

}