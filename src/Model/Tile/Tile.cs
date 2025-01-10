using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Tile : IPlacedOnGrid {

    private Vector2 _gridPosition;
    private Asset _asset;

    public Tile(Asset asset) {
        _asset = asset;
    }

    public Vector2 GetGridPosition() => _gridPosition;
    public void SetGridPosition(Vector2 pos) => _gridPosition = pos;

}