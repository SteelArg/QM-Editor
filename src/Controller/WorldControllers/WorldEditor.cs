using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldEditor : Singleton<WorldEditor> {

    public static IGridObjectFactory ObjectInCursor { get => Instance._objectInCursor; }

    private IGridObjectFactory _objectInCursor;

    public static int[] CursorPositionOnGrid {
        get {
            int[] originalPos = GridRenderSettings.Default.ScreenPositionToGrid(ScreenRenderer.Instance.GetMousePosition());
            return originalPos.ValidateGridPosition();
        }
    }

    public void PlaceObjectOnCursor() {
        if (CursorPositionOnGrid == null || _objectInCursor == null) return;

        World.Instance.Grid.PlaceOnGrid(_objectInCursor.Create(), CursorPositionOnGrid);
    }

    public void ClearCellOnCursor(bool withTile = false) {
        if (CursorPositionOnGrid == null) return;

        Grid grid = World.Instance.Grid;
        GridCell cell = grid.GetGridCell(CursorPositionOnGrid);
        
        foreach (GridObject gridObject in cell.Objects) {
            if (gridObject is Tile && !withTile) continue;
            cell.RemoveObject(gridObject);
        }
    }

    public void SetObjectInCursor(IGridObjectFactory gridObjectFactory) => _objectInCursor = gridObjectFactory;

}