namespace QMEditor.Model;

public class CopyGridObjectFromCellCommand : EditCommandBase {

    private readonly bool _copyTileOnly;

    private GridObject _previousGridObjectInCursor;

    public CopyGridObjectFromCellCommand(EditContext ctx, bool copyTileOnly = false) : base(ctx) {
        _copyTileOnly = copyTileOnly;
    }

    protected override void OnExecute() {
        _previousGridObjectInCursor = World.Cursor.GetObject();
        GridCell cell = World.Instance.Grid.GetGridCell(_context.CursorGridPosition);

        // Tile
        if (_copyTileOnly) {
            World.Cursor.SetCopyOfObject(cell.Tile);
            return;
        }

        // Else (prefer Character)
        GridObject objectToCopy = null;
        foreach (GridObject gridObject in cell.Objects) {
            if (gridObject is Character)
                objectToCopy = gridObject;
        }

        // Any GridObject (non-Tile)
        if (objectToCopy == null && cell.Objects.Length > (cell.Tile == null ? 0 : 1)) {
            foreach (GridObject gridObject in cell.Objects) {
                if (gridObject is Tile) continue;
                objectToCopy = gridObject;
                break;
            }
        }

        World.Cursor.SetCopyOfObject(objectToCopy);
    }

    protected override void OnUndo() {
        World.Cursor.SetObject(_previousGridObjectInCursor);
    }

    protected override bool IsConditionSatisfied() => _context.CursorGridPosition != null;

    public override bool IsEmpty() => !IsConditionSatisfied();

}
