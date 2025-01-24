using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Tile : RenderableGridObject {

    public Tile(Asset asset) : base(asset) {}

    public int GetLift(GridRenderSettings renderSettings) {
        return renderSettings.CalculateTileLift(_asset.GetSize()[1]);
    }

    protected override Vector2 GetRenderPos(GridObjectRenderData renderData) {
        return renderData.RenderSettings.CalculateTilePosition(GridPosition, _asset.GetSize()[1]);
    }

}