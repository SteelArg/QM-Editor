using System;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldEditor : Singleton<WorldEditor> {

    public static IGridObjectFactory ObjectInCursor { get => Instance._objectInCursor; }

    private IGridObjectFactory _objectInCursor;
    private GridObject _gridObjectInCursorPreview;

    public static int[] CursorPositionOnGrid {
        get {
            int[] originalPos = WorldRenderer.RenderSettings.ScreenPositionToGrid(ScreenRenderer.Instance.GetMousePosition());
            return originalPos.ValidateGridPosition();
        }
    }

    public void PlaceObjectOnCursor() {
        if (CursorPositionOnGrid == null || _objectInCursor == null) return;

        World.Instance.Grid.PlaceOnGrid(_objectInCursor.Create(), CursorPositionOnGrid);
    }

    public void ClearCellOnCursor(bool withTile = false) {
        if (CursorPositionOnGrid == null) return;

        Grid grid = World.Instance.Grid;
        GridCell cell = grid.GetGridCell(CursorPositionOnGrid);
        
        foreach (GridObject gridObject in cell.Objects) {
            if (gridObject is Tile && !withTile) continue;
            cell.RemoveObject(gridObject);
        }
    }

    public void CopyGridObjectOnCursor(bool copyTile = false) {
        if (CursorPositionOnGrid == null) return;

        GridCell cell = World.Instance.Grid.GetGridCell(CursorPositionOnGrid);

        // Tile
        if (copyTile) {
            SetObjectInCursor(TileFactory.FromTile(cell.Tile));
            return;
        }

        // Character
        Character character = null;
        foreach (GridObject gridObject in cell.Objects) {
            if (gridObject is Character)
                character = (Character)gridObject;
        }
        SetObjectInCursor(CharacterFactory.FromCharacter(character));
        if (character != null) CharacterEditor.Instance.LoadCharacter(character);
    }

    public void SetObjectInCursor(IGridObjectFactory gridObjectFactory) {
        _objectInCursor = gridObjectFactory;

        if (_objectInCursor == null) {
            _gridObjectInCursorPreview = null;
            return;
        }
        _gridObjectInCursorPreview = _objectInCursor.Create();
    }

    public void Render(SpriteBatch spriteBatch) {
        if (_gridObjectInCursorPreview == null || CursorPositionOnGrid == null) return;
        
        _gridObjectInCursorPreview.SetGridPosition(CursorPositionOnGrid);
        var renderData = new GridObjectRenderData(spriteBatch, WorldRenderer.RenderSettings, 100f);
        renderData.CellLift = World.Instance.Grid.GetGridCell(CursorPositionOnGrid).Tile?.GetLift(renderData.RenderSettings) ?? 0;
        renderData.IsPreview = true;

        _gridObjectInCursorPreview.Render(renderData);
    }

}