using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace QMEditor.Model;

public class Grid {

    private Dictionary<Vector2, GridCell> _cells;
    private Vector2 _gridSize;

    public Grid(Vector2 size) {
        _gridSize = size;
        
        // Grid cells
        _cells = new Dictionary<Vector2, GridCell>();
        for (int x = 0; x < _gridSize.X; x++) {
            for (int y = 0; y < _gridSize.Y; y++) {
                Vector2 pos = new Vector2(x, y);
                _cells[pos] = new GridCell(pos);
            }
        }
    }

    public void PlaceOnGrid(IPlacedOnGrid placableObject, Vector2 gridPosition) {
        _cells[gridPosition].AddObject(placableObject);
    }

    public void MoveOnGrid(IPlacedOnGrid placedObject, Vector2 newGridPosition) {
        _cells[placedObject.GetGridPosition()].RemoveObject(placedObject);
        _cells[newGridPosition].AddObject(placedObject);
    }

    public IPlacedOnGrid[] GetObjectsOnGridPosition(Vector2 pos) {
        return _cells[pos].GetPlacedObjects();
    }

    public GridCell[] GetGridCells() => _cells.Values.ToArray();

}
