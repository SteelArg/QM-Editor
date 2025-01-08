using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;

namespace QMEditor;

public class TabsManager {

    private Desktop _desktop;

    private Tab[] _tabs;
    private int _tabId;

    public TabsManager(Tab[] tabs) {
        _tabs = tabs;
        _tabId = 0;
    }

    public void Render(SpriteBatch spriteBatch) {
        _tabs[_tabId].Draw(spriteBatch);
        //_desktop.Render();
    }

    public void SwitchToTab(int newTabId) {
        if (newTabId == _tabId) return;
        _tabs[_tabId].Close();
        _tabId = newTabId;
        _tabs[_tabId].Open();
    }

}