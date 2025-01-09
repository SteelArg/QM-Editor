using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Controllers;

public abstract class TabsRenderer : Renderer {

    protected TabsManager _tabsManager;

    public TabsRenderer(TabsManager tabsManager) {
        _tabsManager = tabsManager;
    }

}

public class TabsSpriteRenderer : TabsRenderer {
    public TabsSpriteRenderer(TabsManager tabsManager) : base(tabsManager) {}
    public override void Render(SpriteBatch spriteBatch) {
        _tabsManager.Render(spriteBatch);
    }
}

public class TabsUIRenderer : TabsRenderer {
    public TabsUIRenderer(TabsManager tabsManager) : base(tabsManager) {}
    public override void Render(SpriteBatch spriteBatch) {
        _tabsManager.DrawUI();
    }
}