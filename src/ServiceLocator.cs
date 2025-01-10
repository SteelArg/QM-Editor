using QMEditor.Model.IO;

namespace QMEditor;

public static class ServiceLocator {

    public static IFileService FileService { get; private set; }

    static ServiceLocator() {
        FileService = new DebugFileServiceProvider(new EmptyFileServiceProvider());
    }

}