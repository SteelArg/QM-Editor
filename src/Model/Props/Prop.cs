using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Prop : RenderableGridObject {

    public Prop(Asset asset) : base(asset) {}
    
    protected override Vector2 GetRenderPos(GridObjectRenderData renderData) {
        return renderData.RenderSettings.CalculateRenderPosition(GridPosition, _asset.GetSize()) - new Vector2(0f, renderData.CellLift);
    }

}

public class PropFactory : IGridObjectFactory {

    private Asset _asset;

    public PropFactory(Asset asset) {
        _asset = asset;
    }

    public GridObject Create() {
        return new Prop(_asset);
    }

}