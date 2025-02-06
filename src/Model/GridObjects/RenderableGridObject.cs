using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public abstract class RenderableGridObject : GridObject {

    protected AssetBase _asset;
    public AssetBase Asset { get => _asset; }

    public RenderableGridObject(AssetBase asset) {
        _asset = asset;
    }

    public override RenderCommand GetRenderCommand(GridObjectRenderData renderData) {
        float objectDepth = renderData.Depth + renderData.RenderSettings.GetDepthFor(GetType());
        
        RenderCommand renderCommand = new SingleRenderCommand(new SpriteRenderData {
            Texture = _asset.GetTexture(renderData.Frame, renderData.Variation),
            Position = GetRenderPos(renderData),
            Color = renderData.GetObjectColor(),
            Depth = objectDepth
        } );

        return renderCommand;
    }

    public void Render(GridObjectRenderData renderData, SpriteBatch spriteBatch) => GetRenderCommand(renderData).Execute(spriteBatch);

    protected abstract Vector2 GetRenderPos(GridObjectRenderData renderData);

}