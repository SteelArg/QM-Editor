using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public abstract class RenderableGridObject : GridObject {

    protected Asset _asset;
    public Asset Asset { get => _asset; }

    public RenderableGridObject(Asset asset) {
        _asset = asset;
    }

    public override void Render(SpriteBatch spriteBatch, GridRenderSettings renderSettings, float depth) {
        float objectDepth = depth + renderSettings.GetDepthFor(GetType());
        spriteBatch.Draw(_asset.GetTexture(), GetRenderPos(renderSettings), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, objectDepth/1000f);
    }

    protected abstract Vector2 GetRenderPos(GridRenderSettings renderSettings);

}