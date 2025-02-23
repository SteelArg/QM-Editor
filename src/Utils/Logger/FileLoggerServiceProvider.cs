using System;
using System.IO;

namespace QMEditor;

public class FileLoggerServiceProvider : ILoggerService {
    
    private readonly string _logFileName;

    public FileLoggerServiceProvider(string logFileName) {
        _logFileName = logFileName;
        Directory.CreateDirectory(Path.GetDirectoryName(_logFileName));
    }

    public void Log(string message) {
        AppendSectionToFile("LOG", message);
    }

    public void Warning(string message) {
        AppendSectionToFile("WARN", message);
    }

    public void Error(string message) {
        AppendSectionToFile("ERR", message);
    }

    private void AppendSectionToFile(string sectionName, string message) {
        File.AppendAllText(_logFileName, $"====================\n{sectionName} AT {GetTimeStamp()}:\n{message}\n");
    }

    private string GetTimeStamp() {
        return DateTime.Now.ToString();
    }

}