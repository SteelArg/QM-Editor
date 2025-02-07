using System.Collections.Generic;

namespace QMEditor.Model;

public class ClearGridCellCommand : EditCommandBase {

    private readonly bool _clearTile;

    private GridObject[] _removedGridObjects;

    public ClearGridCellCommand(EditContext ctx, bool clearTile = false) : base(ctx) {
        _clearTile = clearTile;
    }

    protected override void OnExecute() {
        GridCell cell = World.Instance.Grid.GetGridCell(_context.CursorGridPosition);
        var removedGridObjects = new List<GridObject>();

        foreach (GridObject gridObject in cell.Objects) {
            if (gridObject is Tile && !_clearTile) continue;
            cell.RemoveObject(gridObject);
            removedGridObjects.Add(gridObject);
        }

        _removedGridObjects = removedGridObjects.ToArray();
    }

    protected override void OnUndo() {
        GridCell cell = World.Instance.Grid.GetGridCell(_context.CursorGridPosition);
        
        foreach (GridObject gridObject in _removedGridObjects) {
            cell.AddObject(gridObject);
        }
    }

    protected override bool IsConditionSatisfied() => _context.CursorGridPosition != null;

    public override bool IsEmpty() => !IsConditionSatisfied() || _removedGridObjects.Length < 1;

}
