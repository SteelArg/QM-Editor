using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class Accessory {

    public AssetBase Asset { get => _asset; }
    public int Lift { get => _lift; }

    private AssetBase _asset;
    private int _lift;

    public Accessory(AssetBase asset, int lift = 0) {
        _asset = asset;
        _lift = lift;
    }

    public Accessory Clone() {
        return new Accessory(_asset, _lift);
    }

    public void SetLift(int lift) => _lift = lift;

    public RenderCommandBase GetRenderCommand(Vector2 centerRenderPos, GridObjectRenderData renderData) {
        Vector2 renderPos = centerRenderPos - new Vector2(_asset.GetSize()[0]/2, _asset.GetSize()[1]);
        renderPos += new Vector2(0f, -_lift);
        
        var renderCommand = new SingleRenderCommand(new SpriteRenderData {
            Texture = _asset.GetTexture(renderData.Frame),
            Position = renderPos,
            Color = renderData.GetObjectColor(),
            Depth = renderData.Depth,
            Flip = renderData.Flip
        });

        return renderCommand;
    }

    public void Render(Vector2 centerRenderPos, GridObjectRenderData renderData, SpriteBatch spriteBatch) => GetRenderCommand(centerRenderPos, renderData).Execute(spriteBatch);

}