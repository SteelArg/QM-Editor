using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace QMEditor.Model;

public class Grid {

    private GridCell[,] _cells;
    private int[] _gridSize;

    public int[] Size { get => _gridSize; }

    public Grid(int[] size) {
        _gridSize = size;
        
        // Grid cells
        _cells = new GridCell[size[0], size[1]];
        LoopThroughPositions.Every((x, y) => {
            _cells[x,y] = new GridCell([x,y]);
        }, _gridSize);
    }

    public void PlaceOnGrid(GridObject placableObject, int[] gridPosition) {
        _cells[gridPosition[0], gridPosition[1]].AddObject(placableObject);
    }

    public void MoveOnGrid(GridObject placedObject, int[] newGridPosition) {
        _cells[placedObject.GridPosition[0], placedObject.GridPosition[1]].RemoveObject(placedObject);
        _cells[newGridPosition[0], newGridPosition[1]].AddObject(placedObject);
    }

    public GridCell GetGridCell(int[] pos) {
        return _cells[pos[0], pos[1]];
    }

    public GridCell[] GetGridCells() => _cells.Cast<GridCell>().ToArray();

}
