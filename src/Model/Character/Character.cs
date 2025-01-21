using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class Character : RenderableGridObject {

    public Accessory[] Accessories { get => _accessories.ToArray(); }

    private List<Accessory> _accessories = new List<Accessory>();

    public Character(Asset asset) : base(asset) {}

    public void AddAccessory(Accessory accessory) => _accessories.Add(accessory);
    public void RemoveAccessory(Accessory accessory) => _accessories.Remove(accessory);

    public override void Render(SpriteBatch spriteBatch, GridRenderSettings renderSettings, float depth, bool hovered = false) {
        base.Render(spriteBatch, renderSettings, depth, hovered);
        Vector2 renderPos = GetRenderPos(renderSettings);
        
        // TODO: Render accessories
        Vector2 renderCenter = renderPos + new Vector2(_asset.GetSize()[0]/2, _asset.GetSize()[1]/2);
        for (int i = 0; i < _accessories.Count; i++) {
            _accessories[i].Render(spriteBatch, renderCenter, depth+renderSettings.GetDepthFor<Accessory>()+0.01f*i, hovered);
        }
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