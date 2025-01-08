using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace QMEditor.Model;

public class GridCell {
    private List<IPlacedOnGrid> _objects;
    private Vector2 _position;
    
    public GridCell(Vector2 pos, List<IPlacedOnGrid> objects = null) {
        _position = pos;
        _objects = new List<IPlacedOnGrid>();
        if (objects == null) return;
        foreach (IPlacedOnGrid obj in objects) {
            AddObject(obj);
        }
    }

    public void AddObject(IPlacedOnGrid placedObject) {
        _objects.Add(placedObject);
        placedObject.SetGridPosition(_position);
    }
    public void RemoveObject(IPlacedOnGrid placedObject) {
        _objects.Remove(placedObject);
    }

    public IPlacedOnGrid[] GetPlacedObjects() {
        return _objects.ToArray();
    }
}
