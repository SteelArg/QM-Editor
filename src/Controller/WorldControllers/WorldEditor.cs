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

    public EditContext GetEditContext() => new EditContext(CursorGridPosition);

    public void Render(SpriteBatch spriteBatch) {
        if (World.Cursor.IsEmpty || CursorGridPosition == null) return;
        
        World.Cursor.GetObject().SetGridPosition(CursorGridPosition);
        var renderData = new GridObjectRenderData(WorldRenderer.RenderSettings, 100f, 0) {
            CellLift = World.Instance.Grid.GetGridCell(CursorGridPosition).Tile?.GetLift(WorldRenderer.RenderSettings) ?? 0,
            IsPreview = true
        };

        World.Cursor.GetObject().GetRenderCommand(renderData).Execute(spriteBatch);
    }

}