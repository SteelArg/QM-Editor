using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model.IO;

public class DefaultFileServiceProvider : IFileService {


    public string Read(string path) {
        return File.ReadAllText(path);
    }

    public void Write(string path, string content) {
        File.WriteAllText(path, content);
    }

    public string[] GetAllFiles(string path, bool fullNames = false) {
        string[] fullFilenames = Directory.GetFiles(path);
        if (fullNames)
            return fullFilenames;

        string[] shortFilenames = new string[fullFilenames.Length];
        for (int i = 0; i < fullFilenames.Length; i++) {
            shortFilenames[i] = Path.GetFileName(fullFilenames[i]);
        }
        return shortFilenames;
    }

    public Texture2D LoadTexture(string path, Game game) {
        var fileStream = new FileStream(path, FileMode.Open);
        return Texture2D.FromStream(game.GraphicsDevice, fileStream);
    }

}