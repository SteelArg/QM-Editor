namespace QMEditor.Model;

public interface IEditCommand {

    public void Do();
    public void Undo();
    public bool IsEmpty();

}

public abstract class EditCommandBase : IEditCommand {

    protected readonly EditContext _context;

    private bool _wasExecuted;

    public EditCommandBase(EditContext ctx) {
        _context = ctx;
    }

    public void Do() {
        if (_wasExecuted || !IsConditionSatisfied()) return;
        OnExecute();
        _wasExecuted = true;
    }

    public void Undo() {
        if (!_wasExecuted || !IsConditionSatisfied()) return;
        OnUndo();
        _wasExecuted = false;
    }

    protected abstract void OnExecute();
    protected abstract void OnUndo();
    protected abstract bool IsConditionSatisfied();
    public abstract bool IsEmpty();

}

public record EditContext (int[] CursorGridPosition = null, bool HoveredThisFrame = false) {

    public static readonly EditContext Empty = new EditContext();

}
