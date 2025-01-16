using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class Character : RenderableGridObject {

    private List<Accessory> _accessories = new List<Accessory>();

    public Character(Asset asset) : base(asset) {}

    public void AddAccessory(Accessory accessory) => _accessories.Add(accessory);
    public void RemoveAccessory(Accessory accessory) => _accessories.Remove(accessory);

    public override void Render(SpriteBatch spriteBatch, GridRenderSettings renderSettings, float depth, bool hovered = false) {
        base.Render(spriteBatch, renderSettings, depth, hovered);
        // TODO: Render accessories
    }

    protected override Vector2 GetRenderPos(GridRenderSettings renderSettings) {
        return renderSettings.CalculateRenderPosition([(int)GridPosition.X, (int)GridPosition.Y], _asset.GetSize());
    }

}