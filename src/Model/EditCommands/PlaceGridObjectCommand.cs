namespace QMEditor.Model;

public class PlaceGridObjectCommand : EditCommandBase {

    private readonly GridObject _placedObject;

    public PlaceGridObjectCommand(EditContext ctx, GridObject objectToPlace) : base(ctx) {
        _placedObject = objectToPlace;
    }

    protected override void OnExecute() {
        World.Instance.Grid.PlaceOnGrid(_placedObject, _context.CursorGridPosition);
    }

    protected override void OnUndo() {
        World.Instance.Grid.GetGridCell(_context.CursorGridPosition).RemoveObject(_placedObject);
    }

    protected override bool IsConditionSatisfied() => _context.CursorGridPosition != null && _placedObject != null;

    public override bool IsEmpty() => !IsConditionSatisfied();

}
