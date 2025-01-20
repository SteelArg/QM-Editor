using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

public class GridObject {

    private int[] _gridPosition;

    public int[] GridPosition {get => _gridPosition;}

    public GridObject() {}

    public void SetGridPosition(int[] pos) {
        _gridPosition = pos;
    }

    public virtual void Render(SpriteBatch spriteBatch, GridRenderSettings renderSettings, float depth, bool hovered = false) {
        // noop
    }

}

public interface IGridObjectFactory {

    public GridObject Create();

}