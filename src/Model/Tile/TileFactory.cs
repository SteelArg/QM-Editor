namespace QMEditor.Model;

public class TileFactory : IGridObjectFactory {

    private Asset _asset;

    public TileFactory(Asset asset) {
        _asset = asset;
    }

    public static TileFactory FromTile(Tile tile) {
        if (tile == null || tile.Asset == null) return null;

        return new TileFactory(tile.Asset);
    }

    public Tile Create() {
        return new Tile(_asset);
    }

    public void FillGrid(Grid grid) {
        LoopThroughPositions.Every((x, y) => {
            grid.PlaceOnGrid(Create(), [x,y]);
        }, grid.Size);
    }

    GridObject IGridObjectFactory.Create() {
        return Create();
    }

}