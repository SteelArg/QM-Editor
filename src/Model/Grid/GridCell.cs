using System.Collections.Generic;

namespace QMEditor.Model;

public class GridCell {

    public int[] Position {get => _position;}
    public GridObject[] Objects {get => _objects.ToArray();}
    public Tile Tile { get => _tile; }

    private List<GridObject> _objects;
    private Tile _tile;
    private int[] _position;
    
    public GridCell(int[] pos, List<GridObject> objects = null) {
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

        if (placedObject is Tile) {
            if (_tile != null) RemoveObject(_tile);
            _tile = (Tile)placedObject;
        }
    }

    public void RemoveObject(GridObject placedObject) {
        _objects.Remove(placedObject);
        if (_tile == placedObject) _tile = null;
    }

}
