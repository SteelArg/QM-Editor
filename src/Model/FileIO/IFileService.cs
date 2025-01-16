using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model.IO;

public interface IFileService {

    public void Write(string path, string content);

    public string Read(string path);

    public string[] GetAllFiles(string path, bool fullNames = false);

    public Texture2D LoadTexture(string path);
    public void SaveAsPng(string path, RenderTarget2D renderTarget, int[] pngSize);

}