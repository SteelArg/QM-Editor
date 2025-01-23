using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class Asset {

    protected readonly string _path;
    protected bool _isLoaded = false;
    private Texture2D _texture;

    public readonly string Name;
    public readonly string NameOfFile;

    public Asset(string path) {
        _path = $"assets\\{path}";
        Name = Path.GetFileNameWithoutExtension(path);
        NameOfFile = Path.GetFileName(path);
    }

    public virtual void Load() {
        _texture = ServiceLocator.FileService.LoadTexture(_path);
        _isLoaded = true;
    }

    public virtual Texture2D GetTexture(int frame = 0) {
        if (!_isLoaded) return null;
        return _texture;
    }

    public virtual int[] GetSize() {
        if (!_isLoaded) return [0,0];
        return [_texture.Width, _texture.Height];
    }

}