using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class Accessory {

    public Asset Asset { get => _asset; }

    private Asset _asset;

    public Accessory(Asset asset) {
        _asset = asset;
    }

    public Accessory Clone() {
        return new Accessory(_asset);
    }

    public void Render(Vector2 centerRenderPos, GridObjectRenderData renderData) {
        Vector2 renderPos = centerRenderPos - new Vector2(_asset.GetSize()[0]/2, _asset.GetSize()[1]/2);
        
        renderData.SpriteBatch.Draw(
            _asset.GetTexture(renderData.Frame), renderPos, null, renderData.IsHovered ? Palette.HoverColor : Color.White,
            0f, Vector2.Zero, 1f, SpriteEffects.None, renderData.Depth/1000f
        );
    }

}