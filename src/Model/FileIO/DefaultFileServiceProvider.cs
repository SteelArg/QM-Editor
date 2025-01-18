using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model.IO;

public class DefaultFileServiceProvider : IFileService {


    public string Read(string path) {
        if (!File.Exists(path)) return string.Empty;
        return File.ReadAllText(path);
    }

    public void Write(string path, string content) {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
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

    public Texture2D LoadTexture(string path) {
        var fileStream = new FileStream(path, FileMode.Open);
        return Texture2D.FromStream(Global.Game.GraphicsDevice, fileStream);
    }
    
    public void SaveAsPng(string path, RenderTarget2D renderTarget, int[] pngSize) {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        using var pngFile = new FileStream(path, FileMode.Create);
            renderTarget.SaveAsPng(pngFile, 1024, 512);
    }

}