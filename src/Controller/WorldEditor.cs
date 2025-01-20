using QMEditor.Model;

namespace QMEditor.Controllers;

public static class WorldEditor {

    public static IGridObjectFactory ObjectInCursor;

    public static int[] CursorPositionOnGrid {
        get {
            int[] originalPos = GridRenderSettings.Default.ScreenPositionToGrid(ScreenRenderer.Instance.GetMousePosition());
            return ValidateGridPosition(originalPos);
        }
    }

    public static void PlaceObjectOnCursor() {
        if (CursorPositionOnGrid == null || ObjectInCursor == null) return;

        World.Instance.Grid.PlaceOnGrid(ObjectInCursor.Create(), CursorPositionOnGrid);
    }

    public static void ClearCellOnCursor(bool withTile = false) {
        if (CursorPositionOnGrid == null) return;

        Grid grid = World.Instance.Grid;
        GridCell cell = grid.GetGridCell(CursorPositionOnGrid);
        
        foreach (GridObject gridObject in cell.Objects) {
            if (gridObject is Tile && !withTile) continue;
            cell.RemoveObject(gridObject);
        }
    }

    private static int[] ValidateGridPosition(int[] originalPos) {
        if (originalPos[0] < 0 || originalPos[0] >= World.Instance.Grid.Size[0]) return null;
        if (originalPos[1] < 0 || originalPos[1] >= World.Instance.Grid.Size[1]) return null;
        return originalPos;
    }

}