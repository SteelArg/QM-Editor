using System;

namespace QMEditor.Model;

public class SharedWorldCursor {

    public bool IsEmpty { get => _object==null; }
    public Action ObjectChanged;

    private GridObject _object;

    public SharedWorldCursor() {}

    public GridObject GetObject() => _object;
    public GridObject GetCopyOfObject() => _object.Clone();
    public void SetCopyOfObject(GridObject gridObject) {
        _object = gridObject?.Clone();
        ObjectChanged?.Invoke();
    }
    public void SetObject(GridObject gridObject) {
        _object = gridObject;
        ObjectChanged?.Invoke();
    }

}