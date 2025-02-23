using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Prop : RenderableGridObject {

    [AddToInspection(InspectionProperty.PropertyType.Check)]
    public bool RenderPostShader { get; set; }

    public Prop(AssetBase asset) : base(asset) {}
    public Prop(Dictionary<string, string> stringData) {
        LoadFromString(stringData);
    }

    public override GridObject Clone() {
        var prop = new Prop(_asset);
        prop.Flip = Flip;
        prop.RenderPostShader = RenderPostShader;
        return prop;
    }

    public override RenderCommandBase GetRenderCommand(GridObjectRenderData renderData) {
        RenderCommandBase renderCommand = base.GetRenderCommand(renderData);
        renderCommand.SetPass(RenderPostShader ? 2 : 0);
        return renderCommand;
    }

    protected override Vector2 GetRenderPos(GridObjectRenderData renderData) {
        return renderData.RenderSettings.CalculateRenderPosition(GridPosition, _asset.GetSize()) - new Vector2(0f, renderData.CellLift);
    }

    public override Dictionary<string, string> SaveToString(Dictionary<string, string> existingData = null){
        existingData = base.SaveToString(existingData);
        existingData.Add("RenderPostShader", RenderPostShader.ToString());
        return existingData;
    }

    protected override void LoadFromString(Dictionary<string, string> stringData) {
        base.LoadFromString(stringData);
        RenderPostShader = bool.Parse(stringData.GetValueOrDefault("RenderPostShader") ?? "False");
    }

}