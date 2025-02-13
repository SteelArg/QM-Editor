using QMEditor.Model.IO;
using QMEditor.View;

namespace QMEditor;

public static class ServiceLocator {

    public static IFileService FileService { get; private set; }
    public static IMessageWindowsService MessageWindowsService { get; private set; }

    static ServiceLocator() {
        FileService = new DefaultFileServiceProvider();
        MessageWindowsService = new DebugMessageWindowsServiceProvider(new DefaultMessageWindowsServiceProvider());
    }

}