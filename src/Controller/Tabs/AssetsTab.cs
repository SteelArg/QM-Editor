using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class AssetsTab : Tab {

    private AssetsLoader _loader;

    public AssetsTab() : base() {
        _loader = new AssetsLoader();
    }

    public override void Load(Game game) {
        base.Load(game);
        _loader.Load(game);
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