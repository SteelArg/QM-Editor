using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Character : RenderableGridObject {

    public Accessory[] Accessories { get => _accessories.ToArray(); }

    private List<Accessory> _accessories = new List<Accessory>();

    public Character(Asset asset) : base(asset) {}

    public void AddAccessory(Accessory accessory) => _accessories.Add(accessory);
    public void RemoveAccessory(Accessory accessory) => _accessories.Remove(accessory);

    public override void Render(GridObjectRenderData renderData) {
        base.Render(renderData);
        Vector2 renderPos = GetRenderPos(renderData);
        
        Vector2 renderCenter = renderPos + new Vector2(_asset.GetSize()[0]/2, _asset.GetSize()[1]/2);
        for (int i = 0; i < _accessories.Count; i++) {
            GridObjectRenderData accessoryRenderData = renderData.WithAddedDepth(renderData.RenderSettings.GetDepthFor<Accessory>()+0.01f*i);
            _accessories[i].Render(renderCenter, accessoryRenderData);
        }
    }

    protected override Vector2 GetRenderPos(GridObjectRenderData renderData) {
        return renderData.RenderSettings.CalculateRenderPosition(GridPosition, _asset.GetSize()) - new Vector2(0f, renderData.CellLift);
    }

}

public class CharacterFactory : IGridObjectFactory {

    private Asset _asset;
    private AccessoryFactory[] _accessoryFactories;

    public CharacterFactory(Asset asset, AccessoryFactory[] accessoryFactories = null) {
        _asset = asset;
        _accessoryFactories = accessoryFactories;
    }

    public static CharacterFactory FromCharacter(Character character) {
        if (character == null || character.Asset == null) return null;
        AccessoryFactory[] accessoryFactories = new AccessoryFactory[character.Accessories.Length];
        for (int i = 0; i < character.Accessories.Length; i++) {
            accessoryFactories[i] = new AccessoryFactory(character.Accessories[i].Asset);
        }
        var factory = new CharacterFactory(character.Asset, accessoryFactories);
        return factory;
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