using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Prop : RenderableGridObject {

    public Prop(Asset asset) : base(asset) {}

    public override GridObject Clone() {
        return new Prop(_asset);
    }

    protected override Vector2 GetRenderPos(GridObjectRenderData renderData) {
        return renderData.RenderSettings.CalculateRenderPosition(GridPosition, _asset.GetSize()) - new Vector2(0f, renderData.CellLift);
    }

}