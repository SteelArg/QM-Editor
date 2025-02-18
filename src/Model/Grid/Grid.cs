using System;
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

    public void MoveRepeating(int x, int y) {
        DoGridCellsPositionOperation((int[] pos) => {
            int newX = (pos[0] + x) % Size[0];
            int newY = (pos[1] + y) % Size[1];
            if (newX < 0) newX += Size[0];
            if (newY < 0) newY += Size[1];
            return [newX, newY];
        });
    }

    public void RotateClockwise() => Rotate((int[] pos) => [pos[1], -pos[0]]);
    public void RotateCounterclockiwise() => Rotate((int[] pos) => [-pos[1], pos[0]]);
    
    private void Rotate(Func<int[], int[]> rotation) {
        DoGridCellsPositionOperation((int[] pos) => {
            int[] relativePosition = [
                Size[0]%2==0 ? pos[0] - Size[0]/2 + (pos[0]>=Size[0]/2?1:0) : pos[0] - (int)MathF.Floor(Size[0]/2f),
                Size[1]%2==0 ? pos[1] - Size[1]/2 + (pos[1]>=Size[1]/2?1:0) : pos[1] - (int)MathF.Floor(Size[1]/2f),
            ];
            int[] newRelativePosition = rotation(relativePosition);
            int[] newPosition = [
                Size[0]%2==0 ? newRelativePosition[0] + Size[0]/2 + (newRelativePosition[0]>0?-1:0) : newRelativePosition[0] + (int)MathF.Floor(Size[0]/2f),
                Size[1]%2==0 ? newRelativePosition[1] + Size[1]/2 + (newRelativePosition[1]>0?-1:0) : newRelativePosition[1] + (int)MathF.Floor(Size[1]/2f),
            ];
            return newPosition;
        });
    }

    public GridCell GetGridCell(int[] pos) => _cells[pos[0], pos[1]];
    public GridCell[] GetGridCells() => _cells.Cast<GridCell>().ToArray();

    private void DoGridCellsPositionOperation(Func<int[], int[]> positionOperation) {
        GridCell[,] newCells = new GridCell[Size[0], Size[1]];
        LoopThroughPositions.Every((x,y) => {
            GridCell cell = _cells[x,y];
            int[] newPosition = positionOperation([x,y]);
            cell.SetPosition(newPosition);
            newCells[newPosition[0], newPosition[1]] = cell;
        }, Size);
        _cells = newCells;
    }

}
