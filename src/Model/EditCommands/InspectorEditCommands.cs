namespace QMEditor.Model;

public class CopyGridObjectCommand : EditCommandBase {

    private readonly GridObject _copiedGridObject;

    private GridObject _previousGridObjectInCursor;

    public CopyGridObjectCommand(GridObject gridObject) : base(EditContext.Empty) {
        _copiedGridObject = gridObject;
    }

    protected override void OnExecute() {
        _previousGridObjectInCursor = World.Cursor.GetObject();

        World.Cursor.SetCopyOfObject(_copiedGridObject);
    }

    protected override void OnUndo() {
        World.Cursor.SetObject(_previousGridObjectInCursor);
    }

    protected override bool IsConditionSatisfied() => true;

    public override bool IsEmpty() => !IsConditionSatisfied();

}

public class DeleteGridObjectCommand : EditCommandBase {

    private readonly GridObject _deletedGridObject;
    private readonly GridCell _cell;

    public DeleteGridObjectCommand(GridObject gridObject) : base(EditContext.Empty) {
        _deletedGridObject = gridObject;
        _cell = World.Instance.Grid.GetGridCell(_deletedGridObject.GridPosition);
    }

    protected override void OnExecute() {
        _cell.RemoveObject(_deletedGridObject);
    }

    protected override void OnUndo() {
        _cell.AddObject(_deletedGridObject);
    }

    protected override bool IsConditionSatisfied() => true;

    public override bool IsEmpty() => !IsConditionSatisfied();

}

