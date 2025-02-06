using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public abstract class AssetBase {

    public string Name { get; private set;}
    public string NameOfFile { get; private set;}

    private bool _isLoaded = false;

    public void Load() {
        LoadTextures();
        _isLoaded = true;
    }

    public Texture2D GetTexture(int frame = 0, string variation = null) {
        if (!_isLoaded) return null;
        return SelectTexture(frame, variation);
    }

    public int[] GetSize() {
        if (!_isLoaded) return [0,0];
        Texture2D texture = SelectTexture(0, null);
        return [texture.Width, texture.Height];
    }

    protected void SetPathAsName(string path) {
        Name = Path.GetFileNameWithoutExtension(path);
        NameOfFile = Path.GetFileName(path);
    }

    protected abstract void LoadTextures();
    protected abstract Texture2D SelectTexture(int frame, string variation);

}