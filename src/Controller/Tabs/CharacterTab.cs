using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;

namespace QMEditor.Controllers;

public class CharacterTab : Tab {

    public CharacterTab() : base() {

    }

    protected override Widget BuildUI() {
        return null;
    }

    public override void Open() {}
    public override void Close() {}
    
    public override void Draw(SpriteBatch spriteBatch) {
        // noop
    }
}