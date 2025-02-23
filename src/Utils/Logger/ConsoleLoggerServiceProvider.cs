using System;

namespace QMEditor;

public class ConsoleLoggerServiceProvider : ILoggerService {

    public void Log(string message) {
        LogSection("LOG");
        Console.WriteLine(message);
    }

    public void Warning(string message) {
        LogSection("WARN");
        Console.WriteLine(message);
    }
    
    public void Error(string message) {
        LogSection("ERR");
        Console.WriteLine(message);
    }

    private void LogSection(string sectionName) {
        Console.WriteLine("====================");
        Console.WriteLine(sectionName);
    }

}