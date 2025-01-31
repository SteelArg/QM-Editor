using System;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class CharacterEditor {

    public Action<Accessory[]> AccessoriesChanged;

    private Character _character;

    public CharacterEditor() {}

    public void SetCharacterAsset(Asset characterAsset) => _character = new Character(characterAsset, _character?.Accessories);
    public void AddAccessoryAsset(Asset accessoryAsset) {
        _character.AddAccessory(new Accessory(accessoryAsset));
        AccessoriesChanged?.Invoke(_character.Accessories);
    }
    public void RemoveAccessory(int accessoryId) {
        _character.RemoveAccessory(accessoryId);
        AccessoriesChanged?.Invoke(_character.Accessories);
    }
    public void ChangeAccessoryLift(int accessoryId, int lift) {
        _character.Accessories[accessoryId].SetLift(lift);
    }
    public void LoadCharacter(Character character) {
        _character = character;
        AccessoriesChanged?.Invoke(_character.Accessories);
    }

    public void Render(SpriteBatch spriteBatch) {
        if (_character == null) return;

        _character.SetGridPosition([0,0]);
        var renderData = new GridObjectRenderData(spriteBatch, GridRenderSettings.CharacterEditor, 0f);
        _character.Render(renderData);
    }

    public Character CreateCharacter() => (Character)_character?.Clone();
    public Character GetCharacter() => _character;

}