﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QMEditor;

public class EditorApp : Game {

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Tab[] _tabs;
    private int _tabId;

    private World _world;

    public EditorApp() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Tabs
        _tabs = new Tab[] { new SettingsTab(), new SceneTab(), new AssetsTab() };
        _tabId = 0;

        _world = new World();
        World.instance.Hello();
    }

    protected override void Initialize() {
        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack);
        _tabs[_tabId].Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void SwitchToTab(int newTabId) {
        if (newTabId == _tabId) return;
        _tabs[_tabId].Close();
        _tabId = newTabId;
        _tabs[_tabId].Open();
    }
}
