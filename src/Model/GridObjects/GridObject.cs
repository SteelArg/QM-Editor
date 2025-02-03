using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

public abstract class GridObject {

    private int[] _gridPosition;

    public int[] GridPosition {get => _gridPosition;}

    public GridObject() {}

    public abstract GridObject Clone();

    public void SetGridPosition(int[] pos) {
        _gridPosition = pos;
    }

    public virtual RenderCommand GetRenderCommand(GridObjectRenderData renderData) => new EmptyRenderCommand();

}

public struct GridObjectRenderData {

    //public readonly SpriteBatch SpriteBatch;
    public readonly GridRenderSettings RenderSettings;
    public float Depth;
    public int Frame;
    public bool IsHovered;
    public bool IsPreview;
    public int CellLift;

    public GridObjectRenderData(GridRenderSettings renderSettings, float depth, int frame = 0, bool isHovered = false, int cellLift = 0, bool isPreview = false) {
        RenderSettings = renderSettings;
        Depth = depth;
        Frame = frame;
        IsHovered = isHovered;
        CellLift = cellLift;
        IsPreview = isPreview;
    }

    public GridObjectRenderData WithAddedDepth(float addedDepth) {
        GridObjectRenderData newRenderData = this with { Depth = Depth + addedDepth };
        return newRenderData;
    }

    public Color GetObjectColor() {
        if (IsPreview)
            return Palette.PlacingColor;
        if (IsHovered)
            return Palette.HoverColor;
        return Color.White;
    }
    
}