using Microsoft.Xna.Framework;
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

    public Texture2D LoadTexture(string path, Game game) {
        return null;
    }
    
}