using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace QMEditor.Model;

public class GridCell {
    private List<GridObject> _objects;
    private Vector2 _position;

    public Vector2 Position {get => _position;}
    public GridObject[] Objects {get => _objects.ToArray();}
    
    public GridCell(Vector2 pos, List<GridObject> objects = null) {
        _position = pos;
        _objects = new List<GridObject>();
        if (objects == null) return;
        foreach (GridObject obj in objects) {
            AddObject(obj);
        }
    }

    public void AddObject(GridObject placedObject) {
        _objects.Add(placedObject);
        placedObject.SetGridPosition(_position);
    }
    public void RemoveObject(GridObject placedObject) {
        _objects.Remove(placedObject);
    }

    public GridObject[] GetPlacedObjects() {
        return _objects.ToArray();
    }
}
