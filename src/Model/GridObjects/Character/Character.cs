using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Character : RenderableGridObject {

    public Accessory[] Accessories { get => _accessories.ToArray(); }
    
    [AddToInspection(InspectionProperty.PropertyType.String)]
    public string Variation { get { return _variation; } set { _variation = value == string.Empty ? "base" : value; } }

    [AddToInspection(InspectionProperty.PropertyType.Check)]
    public bool DrawSilhouette { get {return _drawSilhouette; } set { _drawSilhouette = value; } }

    private List<Accessory> _accessories = new List<Accessory>();
    private string _variation;
    private bool _drawSilhouette;

    public Character(AssetBase asset, Accessory[] accessories = null, string variation = null) : base(asset) {
        if (accessories != null)
            _accessories.AddRange(accessories);
        _variation = variation ?? "base";
    }

    public Character(Dictionary<string, string> stringData) {
        LoadFromString(stringData);
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

        // Base render command
        RenderCommandBase baseRenderCommand = base.GetRenderCommand(renderData);        
        List<RenderCommandBase> commands = [baseRenderCommand];

        // Silhouette render command
        if (_drawSilhouette) {
            RenderCommandBase silhouetteRenderCommand = base.GetRenderCommand(renderData.WithAddedDepth(10f) with { Alpha = 0.3f });
            silhouetteRenderCommand.SetPass(1);
            commands.Add(silhouetteRenderCommand);
        }
        
        // Accessories render commands
        Vector2 renderPos = GetRenderPos(renderData);
        Vector2 renderCenter = renderPos + new Vector2(_asset.GetSize()[0]/2, _asset.GetSize()[1]);
        for (int i = 0; i < _accessories.Count; i++) {
            GridObjectRenderData accessoryRenderData = renderData.WithAddedDepth(renderData.RenderSettings.GetDepthFor<Accessory>()+0.01f*i) with { Flip = Flip };
            commands.Add(_accessories[i].GetRenderCommand(renderCenter, accessoryRenderData));
        }

        return new GroupedRenderCommand(commands.ToArray());
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

    public override Dictionary<string, string> SaveToString(Dictionary<string, string> existingData = null) {
        existingData = base.SaveToString(existingData);
        existingData.Add("Variation", _variation);
        existingData.Add("Sihlouette", _drawSilhouette.ToString());
        
        // Save accessories
        string accessoriesData = "";
        foreach (Accessory accessory in _accessories) {
            accessoriesData += $"{accessory.Asset.Name}#{accessory.Lift},";
        }
        if (accessoriesData.Length > 0)
            accessoriesData = accessoriesData.Remove(accessoriesData.Length-1);
        existingData.Add("Accessories", accessoriesData);

        return existingData;
    }

    protected override void LoadFromString(Dictionary<string, string> stringData) {
        base.LoadFromString(stringData);
        Variation = stringData.GetValueOrDefault("Variation") ?? string.Empty;
        DrawSilhouette = bool.Parse(stringData.GetValueOrDefault("Sihlouette") ?? "False");

        foreach (string accessoryString in stringData.GetValueOrDefault("Accessories")?.Split(',') ?? [string.Empty]) {
            if (accessoryString == string.Empty) continue;
            string accessoryAssetName = accessoryString.Split('#')[0];
            int accessoryLift = int.Parse(accessoryString.Split('#')[1]);
            _accessories.Add(new Accessory(AssetsLoader.Instance.GetAsset(accessoryAssetName, AssetsFolders.Accessories), accessoryLift));
        }
    }

}