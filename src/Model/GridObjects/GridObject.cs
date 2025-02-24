using System.Collections.Generic;
using Microsoft.Xna.Framework;
using QMEditor.Model;

public abstract class GridObject : IInspectable {

    public int[] GridPosition {get => _gridPosition;}

    private int[] _gridPosition;
    private InspectionData _inspectionData;

    public GridObject() {
        _inspectionData = new InspectionData(this);
    }

    public abstract GridObject Clone();

    public void SetGridPosition(int[] pos) {
        _gridPosition = pos;
    }

    public virtual RenderCommandBase GetRenderCommand(GridObjectRenderData renderData) => new EmptyRenderCommand();
    public InspectionData GetInspectionData() => _inspectionData;

    public virtual Dictionary<string, string> SaveToString(Dictionary<string, string> existingData = null) {
        if (existingData == null)
            existingData = new Dictionary<string, string>();
        existingData.Add("Type", GetType().Name);
        return existingData;
    }

    protected virtual void LoadFromString(Dictionary<string, string> stringData) {
        // noop
    }

}

public record GridObjectRenderData(
    GridRenderSettings RenderSettings, float Depth = 0f, int Frame = 0, bool IsHovered = false,
    bool IsPreview = false, float Alpha = 1f, int CellLift = 0, bool Flip = false, string Variation = null, float[] Color = null
) {

    public GridObjectRenderData WithAddedDepth(float addedDepth) {
        GridObjectRenderData newRenderData = this with { Depth = Depth + addedDepth };
        return newRenderData;
    }

    public GridObjectRenderData WithOffsetFrame(int frameOffset) {
        return this with { Frame = Frame + frameOffset };
    }

    public Color GetObjectColor() {
        float[] color = [1f, 1f, 1f, 1f];
        if (IsPreview)
            color = (float[])Palette.PlacingColor.Clone();
        if (IsHovered)
            color = (float[])Palette.HoverColor.Clone();

        color[0] *= Color?[0] ?? 1f;
        color[1] *= Color?[1] ?? 1f;
        color[2] *= Color?[2] ?? 1f;
        color[3] *= Alpha * Color?[3] ?? 1f;

        return Palette.ToColor(color);
    }
    
}