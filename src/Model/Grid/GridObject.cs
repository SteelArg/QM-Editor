using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

public class GridObject {

    private Vector2 _gridPosition;

    public Vector2 GridPosition {get => _gridPosition;}

    public GridObject() {}

    public void SetGridPosition(Vector2 pos) {
        _gridPosition = pos;
    }

    public virtual void Render(SpriteBatch spriteBatch, GridRenderSettings renderSettings, float depth, bool hovered = false) {
        // noop
    }

}