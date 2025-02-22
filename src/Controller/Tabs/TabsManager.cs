using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using QMEditor.View;

namespace QMEditor.Controllers;

public class TabsManager : Singleton<TabsManager> {

    public Tab CurrentTab {get => _tabs[_tabId];}

    private TabSelect _tabSelect;
    private Desktop _desktop;

    private List<Tab> _tabs;
    private Dictionary<Type, Tab> _tabsByType;
    private int _tabId;

    public TabsManager(Tab[] tabs) {
        _tabs = [.. tabs];
        _tabsByType = new Dictionary<Type, Tab>();
        _tabId = -1;
        _tabSelect = new TabSelect();
        _tabSelect.TabSelected += SwitchToTab;

        foreach (Tab tab in tabs) {
            tab.SetManager(this);
            _tabsByType.Add(tab.GetType(), tab);
        }
    }

    public void Load() {
        _desktop = new Desktop();
        Global.SetDesktop(_desktop);

        BuildUI();
        foreach (Tab tab in _tabs) {
            tab.Load();
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

    public void Update(GameTime gameTime) {
        CurrentTab.Update(gameTime);
    }

    public RenderTarget2D Render(SpriteBatch spriteBatch) {
        return CurrentTab.Draw(spriteBatch);
    }

    public void DrawUI() {
        _desktop.Render();
    }

    public void SwitchToTab<T>() where T : Tab {
        Tab tab = GetTab<T>();
        int i = _tabs.IndexOf(tab);
        SwitchToTab(i);
    }

    private void SwitchToTab(int newTabId) {
        if (newTabId == _tabId) return;

        // Tab switch
        if (_tabId > -1)
            CurrentTab.Close();
        _tabId = newTabId;
        CurrentTab.Open();
        
        // Tab UI
        Grid rootGrid = (Grid)_desktop.Root;
        Grid grid = (Grid)rootGrid.Widgets[1];
        grid.Widgets.Clear();
        grid.Widgets.Add(CurrentTab.Widget);
    }

    public T GetTab<T>() where T : Tab {
        return (T)_tabsByType[typeof(T)];
    }

}