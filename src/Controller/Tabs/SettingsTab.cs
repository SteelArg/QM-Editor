using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class SettingsTab : Tab {

    public const int DefaultWorldSize = 8;

    private World _currentWorld;

    public SettingsTab() : base() {
        _currentWorld = new World(DefaultWorldSize, DefaultWorldSize);
    }

    public override void Open() {}
    public override void Close() {}
    
    public override void Draw(SpriteBatch spriteBatch) {
        // noop
    }
}