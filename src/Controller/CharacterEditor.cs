using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class CharacterEditor {

    private Asset _characterAsset;
    private List<Asset> _accessoryAssets;

    private static readonly Vector2 _offset = new Vector2(20f, 10f);

    public CharacterEditor() {
        _accessoryAssets = new List<Asset>();
    }

    public void SetCharacterAsset(Asset characterAsset) => _characterAsset = characterAsset;
    public void AddAccessory(Asset accessoryAsset) => _accessoryAssets.Add(accessoryAsset);
    public void RemoveAccessory(int accessoryId) => _accessoryAssets.RemoveAt(accessoryId);

    public void Render(SpriteBatch spriteBatch) {
        // Works like shit
        
        if (_characterAsset != null)
            spriteBatch.Draw(_characterAsset.GetTexture(), _offset, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);
        
        // Render accessories
        for (int i = 0; i < _accessoryAssets.Count; i++) {
            Asset accessoryAsset = _accessoryAssets[i];
            spriteBatch.Draw(accessoryAsset.GetTexture(), _offset, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f + i/100f);
        }
    }

    public CharacterFactory GetCharacterFactory() {
        if (_characterAsset == null) return null;

        List<AccessoryFactory> accessoryFactories = new List<AccessoryFactory>();
        foreach (Asset accessoryAsset in _accessoryAssets) {
            accessoryFactories.Add(new AccessoryFactory(accessoryAsset));
        }

        return new CharacterFactory(_characterAsset, accessoryFactories.ToArray());
    }

}