using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Prop : RenderableGridObject {

    public Prop(AssetBase asset) : base(asset) {}
    public Prop(Dictionary<string, string> stringData) {
        LoadFromString(stringData);
    }

    public override GridObject Clone() {
        return new Prop(_asset);
    }

    protected override Vector2 GetRenderPos(GridObjectRenderData renderData) {
        return renderData.RenderSettings.CalculateRenderPosition(GridPosition, _asset.GetSize()) - new Vector2(0f, renderData.CellLift);
    }

}