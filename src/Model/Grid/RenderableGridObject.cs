using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public abstract class RenderableGridObject : GridObject {

    protected Asset _asset;
    public Asset Asset { get => _asset; }

    public RenderableGridObject(Asset asset) {
        _asset = asset;
    }

    public override void Render(GridObjectRenderData renderData) {
        float objectDepth = renderData.Depth + renderData.RenderSettings.GetDepthFor(GetType());
        
        renderData.SpriteBatch.Draw(_asset.GetTexture(renderData.Frame),
            GetRenderPos(renderData), null, renderData.IsHovered ? Palette.HoverColor : Color.White,
            0f, Vector2.Zero, 1f, SpriteEffects.None, objectDepth/1000f
        );
    }

    protected abstract Vector2 GetRenderPos(GridObjectRenderData renderData);

}