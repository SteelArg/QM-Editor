using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class Asset {

    private string _path;
    private bool _isLoaded = false;
    private Texture2D _texture;

    public Asset(string path) {
        _path = path;
    }

    public void Load(Game game) {
        var fileStream = new FileStream($"assets\\{_path}", FileMode.Open);
        _texture = Texture2D.FromStream(game.GraphicsDevice, fileStream);
        fileStream.Close();
        _isLoaded = true;
    }

    public Texture2D GetTexture() {
        if (!_isLoaded) return null;
        return _texture;
    }

    public int[] GetSize() {
        if (!_isLoaded) return [0,0];
        return [_texture.Width, _texture.Height];
    }

}