namespace QMEditor.Model;

public class TileFactory : IGridObjectFactory {

    private Asset _asset;

    public TileFactory(Asset asset) {
        _asset = asset;
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