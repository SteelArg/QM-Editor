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
        return renderSettings.CalculateRenderPosition(GridPosition, _asset.GetSize());
    }

}

public class CharacterFactory : IGridObjectFactory {

    private Asset _asset;
    private AccessoryFactory[] _accessoryFactories;

    public CharacterFactory(Asset asset, AccessoryFactory[] accessoryFactories = null) {
        _asset = asset;
        _accessoryFactories = accessoryFactories;
    }

    public GridObject Create() {
        Character character = new Character(_asset);
        
        if (_accessoryFactories != null) {
            foreach (AccessoryFactory accessoryFactory in _accessoryFactories) {
                character.AddAccessory(accessoryFactory.Create());
            }
        }

        return character;
    }

}