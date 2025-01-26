using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
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
        _characterEditorView.CreateCharacterClicked += OnCreateCharacterClicked;
        _characterEditorView.RemoveAccessoryClicked += OnRemoveAccessoryClicked;
        _characterEditor.AccessoriesChanged += (accessories) => { _characterEditorView.SetAccessories(accessories.Select(a => a.Asset.Name).ToArray()); };
        return _characterEditorView.BuildUI();
    }

    public void OnCreateCharacterClicked() {
        WorldEditor.Instance.SetObjectInCursor(_characterEditor.GetCharacter());
        _manager.SwitchToTab(1);
    }

    public void OnRemoveAccessoryClicked(int accessoryId) {
        _characterEditor.RemoveAccessory(accessoryId);
    }

    public override void Open() {}
    public override void Close() {}
    
    public override void Draw(SpriteBatch spriteBatch) {
        _characterEditor.Render(spriteBatch);
    }
}