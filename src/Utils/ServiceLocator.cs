using System.IO;
using QMEditor.Model.IO;
using QMEditor.View;

namespace QMEditor;

public static class ServiceLocator {

    public static IFileService FileService { get; private set; }
    public static IMessageWindowsService MessageWindowsService { get; private set; }
    public static ILoggerService LoggerService { get; private set; }

    static ServiceLocator() {
        FileService = new DefaultFileServiceProvider();
        MessageWindowsService = new DebugMessageWindowsServiceProvider(new DefaultMessageWindowsServiceProvider());
        LoggerService = new PartialLoggerServiceProvider(
            new MultipleLoggersServiceProvider([
                new FileLoggerServiceProvider("logs\\session.log"), new ConsoleLoggerServiceProvider()
            ]),
            LogLevel.All
        );
    }

}