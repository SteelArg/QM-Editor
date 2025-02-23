using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public abstract class RenderableGridObject : GridObject {

    [AddToInspection(InspectionProperty.PropertyType.Check)]
    public bool Flip { get; set; }

    protected AssetBase _asset;
    public AssetBase Asset { get => _asset; private set { _asset = value; GetInspectionData()?.SetName(_asset.Name);} }

    public RenderableGridObject(AssetBase asset) : base() {
        Asset = asset;
    }
    public RenderableGridObject() : base() {}

    public override RenderCommandBase GetRenderCommand(GridObjectRenderData renderData) {
        float objectDepth = renderData.Depth + renderData.RenderSettings.GetDepthFor(GetType());
        
        RenderCommandBase renderCommand = new SingleRenderCommand(new SpriteRenderData {
            Texture = _asset.GetTexture(renderData.Frame, renderData.Variation),
            Position = GetRenderPos(renderData),
            Color = renderData.GetObjectColor(),
            Depth = objectDepth,
            Flip = Flip
        }, 0);

        return renderCommand;
    }

    public void Render(GridObjectRenderData renderData, SpriteBatch spriteBatch) => GetRenderCommand(renderData).Execute(spriteBatch);

    protected abstract Vector2 GetRenderPos(GridObjectRenderData renderData);

    public override Dictionary<string, string> SaveToString(Dictionary<string, string> existingData = null) {
        existingData = base.SaveToString(existingData);
        existingData.Add("AssetName", _asset.Name);
        existingData.Add("Flip", Flip.ToString());
        return existingData;
    }

    protected override void LoadFromString(Dictionary<string, string> stringData) {
        base.LoadFromString(stringData);
        Asset = AssetsLoader.Instance.GetAsset(stringData.GetValueOrDefault("AssetName"), AssetsFoldersHelper.FoldersByObjectType(this));
        Flip = bool.Parse(stringData.GetValueOrDefault("Flip") ?? "False");
    }

}