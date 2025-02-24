using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldEditor {

    public static int[] CursorGridPosition {
        get {
            int[] originalPos = WorldRenderer.RenderSettings.ScreenPositionToGrid(ScreenRenderer.Instance.GetMousePosition());
            return originalPos.ValidateGridPosition();
        }
    }

    private int[] _previousCursorGridPosition;

    public EditContext GetEditContext() {
        var editContext = new EditContext(CursorGridPosition, !_previousCursorGridPosition.IsSameAs(CursorGridPosition));
        _previousCursorGridPosition = CursorGridPosition;
        return editContext;
    }

    public void Render(SpriteBatch spriteBatch) {
        if (World.Cursor.IsEmpty || CursorGridPosition == null) return;
        
        World.Cursor.GetObject().SetGridPosition(CursorGridPosition);
        var renderData = new GridObjectRenderData(WorldRenderer.RenderSettings) {
            Depth = 100f,
            CellLift = World.Instance.Grid.GetGridCell(CursorGridPosition).Tile?.GetLift(WorldRenderer.RenderSettings) ?? 0,
            IsPreview = true
        };

        World.Cursor.GetObject().GetRenderCommand(renderData).Execute(spriteBatch);
    }

}