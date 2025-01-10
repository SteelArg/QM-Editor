namespace QMEditor.Model.IO;

public interface IFileService {

    public void Write(string path, string content);

    public string Read(string path);

}