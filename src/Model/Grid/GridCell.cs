using System.Collections.Generic;

namespace QMEditor.Model;

public class GridCell : IInspectable {

    [AddToInspection(InspectionProperty.PropertyType.GridPosition, AddToInspectionAttribute.AccessMode.ReadOnly)]
    public int[] Position { get => _position; }
    
    [AddToInspection(InspectionProperty.PropertyType.InspectablesLinkArray)]
    public GridObject[] Objects { get => _objects.ToArray(); }
    
    public Tile Tile { get => _tile; }

    private List<GridObject> _objects;
    private Tile _tile;
    private int[] _position;
    private InspectionData _inspectionData;
 
    public GridCell(int[] pos, List<GridObject> objects = null) {
        _position = pos;
        _objects = new List<GridObject>();
        _inspectionData = new InspectionData(this);
        if (objects == null) return;
        foreach (GridObject obj in objects) {
            AddObject(obj);
        }
    }

    public void SetPosition(int[] pos) {
        _position = pos;
        foreach (GridObject obj in _objects) {
            obj.SetGridPosition(pos);
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

    public InspectionData GetInspectionData() => _inspectionData;

}
