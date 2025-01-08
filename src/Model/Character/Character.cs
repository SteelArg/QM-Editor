using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Character : IPlacedOnGrid {

    private Vector2 _gridPosition;

    public Character() {}

    public void SetGridPosition(Vector2 pos) {
        _gridPosition = pos;
    }
    public Vector2 GetGridPosition() => _gridPosition;

}