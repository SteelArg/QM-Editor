using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public interface IPlacedOnGrid {

    public void SetGridPosition(Vector2 pos);
    public Vector2 GetGridPosition();

}