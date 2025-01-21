using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class Accessory {

    public Asset Asset { get => _asset; }

    private Asset _asset;

    public Accessory(Asset asset) {
        _asset = asset;
    }

    public void Render(SpriteBatch spriteBatch, Vector2 centerRenderPos, float depth, bool hovered) {
        Vector2 renderPos = centerRenderPos - new Vector2(_asset.GetSize()[0]/2, _asset.GetSize()[1]/2);
        spriteBatch.Draw(_asset.GetTexture(), renderPos, null, hovered ? Palette.HoverColor : Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth/1000f);
    }

}

public class AccessoryFactory {

    private Asset _asset;

    public AccessoryFactory(Asset asset) {
        _asset = asset;
    }

    public Accessory Create() {
        return new Accessory(_asset);
    }

}