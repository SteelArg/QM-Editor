using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Tile : RenderableGridObject {

    public Tile(Asset asset) : base(asset) {}

    protected override Vector2 GetRenderPos(GridRenderSettings renderSettings) {
        return renderSettings.CalculateTilePosition([(int)GridPosition.X, (int)GridPosition.Y]);
    }

}