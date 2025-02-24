using QMEditor.Model;

public class ClearCursorCommand : EditCommandBase {

    private GridObject _cursorObject;
    private readonly bool _cursorWasEmpty;

    public ClearCursorCommand(EditContext ctx) : base(ctx) {
        _cursorWasEmpty = World.Cursor.IsEmpty;
    }

    protected override void OnExecute() {
        _cursorObject = World.Cursor.GetObject();
        World.Cursor.SetObject(null);
    }

    protected override void OnUndo() {
        World.Cursor.SetObject(_cursorObject);
    }

    protected override bool IsConditionSatisfied() => !_cursorWasEmpty;

    public override bool IsEmpty() => !IsConditionSatisfied();

}