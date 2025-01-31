using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class Accessory {

    public Asset Asset { get => _asset; }
    public int Lift { get => _lift; }

    private Asset _asset;
    private int _lift;

    public Accessory(Asset asset, int lift = 0) {
        _asset = asset;
        _lift = lift;
    }

    public Accessory Clone() {
        return new Accessory(_asset, _lift);
    }

    public void SetLift(int lift) => _lift = lift;

    public void Render(Vector2 centerRenderPos, GridObjectRenderData renderData) {
        Vector2 renderPos = centerRenderPos - new Vector2(_asset.GetSize()[0]/2, _asset.GetSize()[1]);
        renderPos += new Vector2(0f, -_lift);
        
        renderData.SpriteBatch.Draw(
            _asset.GetTexture(renderData.Frame), renderPos, null, renderData.IsHovered ? Palette.HoverColor : Color.White,
            0f, Vector2.Zero, 1f, SpriteEffects.None, renderData.Depth/1000f
        );
    }

}