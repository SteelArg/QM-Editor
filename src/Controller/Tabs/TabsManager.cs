using System;
using Microsoft.Xna.Framework;
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
        _tabId = -1;
        _tabSelect = new TabSelect();
        _tabSelect.TabSelected += SwitchToTab;
    }

    public void Load(Game game) {
        _desktop = new Desktop();
        BuildUI();
        foreach (Tab tab in _tabs) {
            tab.Load(game);
        }
        SwitchToTab(0);
    }

    public void BuildUI() {
        var rootGrid = new Grid();
        rootGrid.RowsProportions.Add(new Proportion(ProportionType.Pixels, AppLayout.TabSelectHeight));
        rootGrid.RowsProportions.Add(new Proportion(ProportionType.Fill));
        Widget tabSelection = _tabSelect.BuildUI();
        rootGrid.Widgets.Add(tabSelection);
        var grid = new Grid();
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Fill));
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, AppLayout.InspectorWidth));
        Grid.SetRow(grid, 1);
        rootGrid.Widgets.Add(grid);
        _desktop.Root = rootGrid;
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
        if (_tabId > -1)
            CurrentTab.Close();
        _tabId = newTabId;
        CurrentTab.Open();
        Grid rootGrid = (Grid)_desktop.Root;
        Grid grid = (Grid)rootGrid.Widgets[1];
        grid.Widgets.Clear();
        grid.Widgets.Add(CurrentTab.Widget);
    }

}