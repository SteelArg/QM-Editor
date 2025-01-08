using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;

namespace QMEditor;

public class EditorApp : Game {

    private const int DEFAULT_WORLD_SIZE = 8;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Desktop _desktop;

    private Tab[] _tabs;
    private int _tabId;
    private World _world;

    public EditorApp() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Tabs
        _tabs = [new SettingsTab(), new SceneTab(), new AssetsTab()];
        _tabId = 0;

        _world = new World(DEFAULT_WORLD_SIZE, DEFAULT_WORLD_SIZE);
        World.instance.Hello();
    }

    protected override void Initialize() {
        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _desktop = new Desktop();
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

        _desktop.Render();

        base.Draw(gameTime);
    }

    public void SwitchToTab(int newTabId) {
        if (newTabId == _tabId) return;
        _tabs[_tabId].Close();
        _tabId = newTabId;
        _tabs[_tabId].Open();
    }
}
