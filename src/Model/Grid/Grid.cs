using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace QMEditor.Model;

public class Grid {

    private Dictionary<Vector2, GridCell> _cells;
    private Vector2 _gridSize;

    public Vector2 Size { get => _gridSize; }

    public Grid(Vector2 size) {
        _gridSize = size;
        
        // Grid cells
        _cells = new Dictionary<Vector2, GridCell>();
        LoopThroughPositions.Every((x, y) => {
            Vector2 pos = new Vector2(x, y);
            _cells[pos] = new GridCell(pos);
        }, _gridSize);
    }

    public void PlaceOnGrid(GridObject placableObject, Vector2 gridPosition) {
        _cells[gridPosition].AddObject(placableObject);
    }

    public void MoveOnGrid(GridObject placedObject, Vector2 newGridPosition) {
        _cells[placedObject.GridPosition].RemoveObject(placedObject);
        _cells[newGridPosition].AddObject(placedObject);
    }

    public GridObject[] GetObjectsOnGridPosition(Vector2 pos) {
        return _cells[pos].GetPlacedObjects();
    }

    public GridCell[] GetGridCells() => _cells.Values.ToArray();

}
