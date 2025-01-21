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
    }

    protected override Widget BuildUI() {
        _characterEditorView.BuildUI();
        _characterEditorView.CharacterCreated += OnCharacterCreated;
        _characterEditorView.AccessoryRemoved += OnAccessoryRemoved;
        return _characterEditorView.Widget;
    }

    public void SelectCharacter(Asset characterAsset) {
        _characterEditor.SetCharacterAsset(characterAsset);
    }

    public void SelectAccessory(Asset accessoryAsset) {
        _characterEditor.AddAccessory(accessoryAsset);
        _characterEditorView.AddAccessory(accessoryAsset.Name);
    }

    public void OnCharacterCreated() {
        WorldEditor.ObjectInCursor = _characterEditor.GetCharacterFactory();
        _manager.SwitchToTab(1);
    }

    public void OnAccessoryRemoved(int accessoryId) {
        _characterEditor.RemoveAccessory(accessoryId);
        _characterEditorView.RemoveAccessory(accessoryId);
    }

    public override void Open() {}
    public override void Close() {}
    
    public override void Draw(SpriteBatch spriteBatch) {
        _characterEditor.Render(spriteBatch);
    }
}