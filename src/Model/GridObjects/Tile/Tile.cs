using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Tile : RenderableGridObject {

    public Tile(AssetBase asset) : base(asset) {}
    public Tile(Dictionary<string, string> stringData) {
        LoadFromString(stringData);
    }

    public override GridObject Clone() {
        return new Tile(_asset);
    }

    public int GetLift(GridRenderSettings renderSettings) {
        return renderSettings.CalculateTileLift(_asset.GetSize()[1]);
    }

    protected override Vector2 GetRenderPos(GridObjectRenderData renderData) {
        return renderData.RenderSettings.CalculateTilePosition(GridPosition, _asset.GetSize()[1]);
    }

}

public class TileFactory {

    private Tile _tile;

    public TileFactory(Tile tile) {
        _tile = tile;
    }

    public TileFactory(AssetBase asset) {
        _tile = new Tile(asset);
    }

    public Tile Create() => (Tile)_tile.Clone();

    public void FillGrid(Grid grid) {
        LoopThroughPositions.Every((x,y) => {
            grid.PlaceOnGrid(_tile.Clone(), [x,y]);
        }, grid.Size);
    }

}