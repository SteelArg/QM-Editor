namespace QMEditor.Model.IO;

public class EmptyFileServiceProvider : IFileService {

    public string Read(string path) {
        return string.Empty;
    }

    public void Write(string path, string content) {
        // noop
    }

}