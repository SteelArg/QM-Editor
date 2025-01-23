using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model.IO;

public class EmptyFileServiceProvider : IFileService {
    

    public string Read(string path) {
        return string.Empty;
    }

    public void Write(string path, string content) {
        // noop
    }

    public string[] GetAllFiles(string path, bool fullNames = false) {
        return [];
    }

    public Texture2D LoadTexture(string path) {
        return null;
    }

    public Texture2D[] LoadGif(string path) {
        return null;
    }

    public void SaveAsGif(string path, RenderTarget2D[] renderTargets, int[] gifSize, int frameDelay) {
        // noop
    }
    
}