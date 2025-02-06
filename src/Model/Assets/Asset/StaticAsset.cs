using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class StaticAsset : AssetBase {

    protected readonly string _path;
    private Texture2D _texture;

    public StaticAsset(string path) {
        _path = $"assets\\{path}";
        SetPathAsName(path);
    }

    protected override void LoadTextures() {
        _texture = ServiceLocator.FileService.LoadTexture(_path);
    }

    protected override Texture2D SelectTexture(int frame, string variation) {
        return _texture;
    }

}