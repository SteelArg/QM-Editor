using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor.Controllers;

public class CharacterTab : Tab {

    private CharacterEditor _characterEditor;
    private CharacterEditorView _characterEditorView;

    public CharacterTab() : base() {
        _characterEditor = new CharacterEditor();
        _characterEditorView = new CharacterEditorView();
        World.Cursor.ObjectChanged += () => {
            if (!World.Cursor.IsEmpty && World.Cursor.GetObject().GetType() == typeof(Character))
                _characterEditor.LoadCharacter((Character)World.Cursor.GetCopyOfObject());
        };
    }

    protected override Widget BuildUI() {
        _characterEditorView.CreateCharacterClicked += OnCreateCharacterClicked;
        _characterEditorView.RemoveAccessoryClicked += OnRemoveAccessoryClicked;
        _characterEditorView.AccessoryLiftChanged += _characterEditor.ChangeAccessoryLift;
        _characterEditorView.VariationSelected += (v) => { _characterEditor.GetCharacter().Variation = v; };
        _characterEditor.AccessoriesChanged += (accessories) => { _characterEditorView.SetAccessories(accessories.Select(a => (a.Asset.Name, a.Lift)).ToArray()); };
        _characterEditor.CharacterChanged += (Character c) => { _characterEditorView.SetVariations(c.GetPossibleVariations()); };
        return _characterEditorView.BuildUI();
    }

    public void OnCreateCharacterClicked() {
        Character character = _characterEditor.GetCharacter();
        if (character == null) {
            _manager.SwitchToTab<AssetsTab>();
            return;
        }
        World.Cursor.SetCopyOfObject(character);
        _manager.SwitchToTab<SceneTab>();
    }

    public void OnRemoveAccessoryClicked(int accessoryId) {
        _characterEditor.RemoveAccessory(accessoryId);
    }

    public void SetCharacterAsset(AssetBase characterAsset) => _characterEditor.SetCharacterAsset(characterAsset);
    public void AddAccessoryAsset(AssetBase accessoryAsset) =>  _characterEditor.AddAccessoryAsset(accessoryAsset);

    public override void Open() {}
    public override void Close() {}

    public override void Draw(SpriteBatch spriteBatch) {
        _characterEditor.Render(spriteBatch);
    }
}