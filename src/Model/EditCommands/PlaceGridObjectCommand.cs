namespace QMEditor.Model;

public class PlaceGridObjectCommand : EditCommandBase {

    private readonly GridObject _placedObject;

    private Tile _replacedTile;

    public PlaceGridObjectCommand(EditContext ctx, GridObject objectToPlace) : base(ctx) {
        _placedObject = objectToPlace;
    }

    protected override void OnExecute() {
        if (_placedObject is Tile)
            _replacedTile = World.Instance.Grid.GetGridCell(_context.CursorGridPosition).Tile;

        World.Instance.Grid.PlaceOnGrid(_placedObject, _context.CursorGridPosition);
    }

    protected override void OnUndo() {
        World.Instance.Grid.GetGridCell(_context.CursorGridPosition).RemoveObject(_placedObject);
        
        if (_replacedTile != null)
            World.Instance.Grid.PlaceOnGrid(_replacedTile, _context.CursorGridPosition);
    }

    protected override bool IsConditionSatisfied() => _context.CursorGridPosition != null && _placedObject != null;

    public override bool IsEmpty() => !IsConditionSatisfied();

}
