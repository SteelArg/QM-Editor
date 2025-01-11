using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class TileFactory {

    private Asset _asset;

    public TileFactory(Asset asset) {
        _asset = asset;
    }

    public Tile Create() {
        return new Tile(_asset);
    }

    public void FillGrid(Grid grid) {
        LoopThroughPositions.Every((x, y) => {
            grid.PlaceOnGrid(Create(), new Vector2(x, y));
        }, grid.Size);
    }

}