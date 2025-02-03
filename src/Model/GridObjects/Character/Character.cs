using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Character : RenderableGridObject {

    public Accessory[] Accessories { get => _accessories.ToArray(); }

    private List<Accessory> _accessories = new List<Accessory>();

    public Character(Asset asset, Accessory[] accessories = null) : base(asset) {
        if (accessories != null)
            _accessories.AddRange(accessories);
    }

    public override GridObject Clone() {
        var character = new Character(_asset);
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

    public override RenderCommand GetRenderCommand(GridObjectRenderData renderData) {
        RenderCommand baseRenderCommand = base.GetRenderCommand(renderData);
        Vector2 renderPos = GetRenderPos(renderData);
        RenderCommand[] commands = new RenderCommand[_accessories.Count+1];
        commands[0] = baseRenderCommand;
        
        Vector2 renderCenter = renderPos + new Vector2(_asset.GetSize()[0]/2, _asset.GetSize()[1]);
        for (int i = 0; i < _accessories.Count; i++) {
            GridObjectRenderData accessoryRenderData = renderData.WithAddedDepth(renderData.RenderSettings.GetDepthFor<Accessory>()+0.01f*i);
            commands[i+1] = _accessories[i].GetRenderCommand(renderCenter, accessoryRenderData);
        }

        return new GroupedRenderCommand(commands);
    }

    protected override Vector2 GetRenderPos(GridObjectRenderData renderData) {
        return renderData.RenderSettings.CalculateRenderPosition(GridPosition, _asset.GetSize()) - new Vector2(0f, renderData.CellLift);
    }

}