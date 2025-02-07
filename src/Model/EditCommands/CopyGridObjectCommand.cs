namespace QMEditor.Model;

public class CopyGridObjectCommand : EditCommandBase {

    private readonly bool _copyTileOnly;

    private GridObject _previousGridObjectInCursor;

    public CopyGridObjectCommand(EditContext ctx, bool copyTileOnly = false) : base(ctx) {
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

        // Else (Character)
        Character character = null;
        foreach (GridObject gridObject in cell.Objects) {
            if (gridObject is Character)
                character = (Character)gridObject;
        }
        World.Cursor.SetCopyOfObject(character);
    }

    protected override void OnUndo() {
        World.Cursor.SetObject(_previousGridObjectInCursor);
    }

    protected override bool IsConditionSatisfied() => _context.CursorGridPosition != null;

    public override bool IsEmpty() => !IsConditionSatisfied();

}
