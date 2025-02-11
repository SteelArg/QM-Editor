using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Character : RenderableGridObject {

    public Accessory[] Accessories { get => _accessories.ToArray(); }
    
    [AddToInspection(InspectionProperty.PropertyType.String)]
    public string Variation { get { return _variation; } set { _variation = value == string.Empty ? "base" : value; } }

    private List<Accessory> _accessories = new List<Accessory>();
    private string _variation;

    public Character(AssetBase asset, Accessory[] accessories = null, string variation = null) : base(asset) {
        if (accessories != null)
            _accessories.AddRange(accessories);
        _variation = variation ?? "base";
    }

    public override GridObject Clone() {
        var character = new Character(_asset, null, _variation);
        character._accessories = new List<Accessory>();
        foreach (Accessory accessory in _accessories) {
            character._accessories.Add(accessory.Clone());
        }
        character.SetGridPosition(GridPosition);
        return character;
    }

    public void AddAccessory(Accessory accessory) => _accessories.Add(accessory);
    public void RemoveAccessory(int accessoryId) => _accessories.RemoveAt(accessoryId);
    public void ClearAccessories() => _accessories.Clear();

    public override RenderCommandBase GetRenderCommand(GridObjectRenderData renderData) {
        renderData = renderData with { Variation = _variation };

        RenderCommandBase baseRenderCommand = base.GetRenderCommand(renderData);

        Vector2 renderPos = GetRenderPos(renderData);
        RenderCommandBase[] commands = new RenderCommandBase[_accessories.Count+1];
        commands[0] = baseRenderCommand;
        
        Vector2 renderCenter = renderPos + new Vector2(_asset.GetSize()[0]/2, _asset.GetSize()[1]);
        for (int i = 0; i < _accessories.Count; i++) {
            GridObjectRenderData accessoryRenderData = renderData.WithAddedDepth(renderData.RenderSettings.GetDepthFor<Accessory>()+0.01f*i) with { Flip = Flip };
            commands[i+1] = _accessories[i].GetRenderCommand(renderCenter, accessoryRenderData);
        }

        return new GroupedRenderCommand(commands);
    }

    protected override Vector2 GetRenderPos(GridObjectRenderData renderData) {
        return renderData.RenderSettings.CalculateRenderPosition(GridPosition, _asset.GetSize()) - new Vector2(0f, renderData.CellLift);
    }

    public string[] GetPossibleVariations() {
        if (Asset is VariatedAsset variatedAsset) {
            return variatedAsset.GetPossibleVariations();
        }
        return ["base"];
    }

}