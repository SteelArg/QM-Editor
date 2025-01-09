using System;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using QMEditor.View;

namespace QMEditor.Controllers;

public class TabsManager {

    public Tab CurrentTab {get => _tabs[_tabId];}

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
        BuildUI();
    }

    public void BuildUI() {
        var grid = new Grid();
        grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, AppLayout.TabSelectHeight));
        Widget tabSelection = _tabSelect.BuildUI();
        grid.Widgets.Add(tabSelection);
        _desktop.Root = grid;
    }

    public void Render(SpriteBatch spriteBatch) {
        CurrentTab.Draw(spriteBatch);
    }

    public void DrawUI() {
        _desktop.Render();
    }

    public void SwitchToTab(int newTabId) {
        if (newTabId == _tabId) return;
        Console.WriteLine($"Switched to tab {newTabId}");
        CurrentTab.Close();
        _tabId = newTabId;
        CurrentTab.Open();
    }

}