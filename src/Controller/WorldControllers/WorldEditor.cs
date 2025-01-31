using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldEditor {

    public static int[] CursorPositionOnGrid {
        get {
            int[] originalPos = WorldRenderer.RenderSettings.ScreenPositionToGrid(ScreenRenderer.Instance.GetMousePosition());
            return originalPos.ValidateGridPosition();
        }
    }

    public void PlaceObject() {
        if (CursorPositionOnGrid == null || World.Cursor.IsEmpty) return;

        World.Instance.Grid.PlaceOnGrid(World.Cursor.GetCopyOfObject(), CursorPositionOnGrid);
    }

    public void ClearCell(bool withTile = false) {
        if (CursorPositionOnGrid == null) return;

        Grid grid = World.Instance.Grid;
        GridCell cell = grid.GetGridCell(CursorPositionOnGrid);
        
        foreach (GridObject gridObject in cell.Objects) {
            if (gridObject is Tile && !withTile) continue;
            cell.RemoveObject(gridObject);
        }
    }

    public void CopyGridObject(bool copyTile = false) {
        if (CursorPositionOnGrid == null) return;

        GridCell cell = World.Instance.Grid.GetGridCell(CursorPositionOnGrid);

        // Tile
        if (copyTile) {
            World.Cursor.SetCopyOfObject(cell.Tile);
            return;
        }

        // Character
        Character character = null;
        foreach (GridObject gridObject in cell.Objects) {
            if (gridObject is Character)
                character = (Character)gridObject;
        }
        World.Cursor.SetCopyOfObject(character);
    }

    public void Render(SpriteBatch spriteBatch) {
        if (World.Cursor.IsEmpty || CursorPositionOnGrid == null) return;
        
        World.Cursor.GetObject().SetGridPosition(CursorPositionOnGrid);
        var renderData = new GridObjectRenderData(spriteBatch, WorldRenderer.RenderSettings, 100f);
        renderData.CellLift = World.Instance.Grid.GetGridCell(CursorPositionOnGrid).Tile?.GetLift(renderData.RenderSettings) ?? 0;
        renderData.IsPreview = true;

        World.Cursor.GetObject().Render(renderData);
    }

}