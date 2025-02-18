namespace QMEditor.Model;

public class MoveGridCommand : EditCommandBase {

    private readonly int[] _moveOffset;

    public MoveGridCommand(EditContext ctx, int[] offset) : base(ctx) {
        _moveOffset = offset;
    }

    protected override void OnExecute() {
        World.Instance.Grid.MoveRepeating(_moveOffset[0], _moveOffset[1]);
    }

    protected override void OnUndo() {
        World.Instance.Grid.MoveRepeating(-_moveOffset[0], -_moveOffset[1]);
    }

    protected override bool IsConditionSatisfied() => true;
    public override bool IsEmpty() => !IsConditionSatisfied();

}

public class RotateGridCommand : EditCommandBase {

    private readonly bool _clockwise;

    public RotateGridCommand(EditContext ctx, bool clockwise) : base(ctx) {
        _clockwise = clockwise;
    }

    protected override void OnExecute() {
        if (_clockwise)
            World.Instance.Grid.RotateClockwise();
        else
            World.Instance.Grid.RotateCounterclockiwise();
    }

    protected override void OnUndo() {
        if (_clockwise)
            World.Instance.Grid.RotateCounterclockiwise();
        else
            World.Instance.Grid.RotateClockwise();
    }

    protected override bool IsConditionSatisfied() => true;
    public override bool IsEmpty() => !IsConditionSatisfied();

}
