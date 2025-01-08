using System;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using QMEditor.View;

namespace QMEditor.Controllers;

public class TabsManager {

    private TabSelect _tabSelect;
    private Desktop _desktop;

    private Tab[] _tabs;
    private int _tabId;

    public TabsManager(Tab[] tabs) {
        _tabs = tabs;
        _tabId = 0;
        _tabSelect = new TabSelect();
        _tabSelect.TabSelected += SwitchToTab;
    }

    public void Load() {
        _desktop = new Desktop();
        var grid = new Grid();
        grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, AppLayout.TabSelectHeight));

        Widget tabSelection = _tabSelect.BuildUI();
        grid.Widgets.Add(tabSelection);
        
        _desktop.Root = grid;
    }

    public void Render(SpriteBatch spriteBatch) {
        _desktop.Render();
        _tabs[_tabId].Draw(spriteBatch);
    }

    public void SwitchToTab(int newTabId) {
        if (newTabId == _tabId) return;
        Console.WriteLine($"Switched to tab {newTabId}");
        _tabs[_tabId].Close();
        _tabId = newTabId;
        _tabs[_tabId].Open();
    }

}