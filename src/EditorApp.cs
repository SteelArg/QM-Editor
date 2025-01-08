using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QMEditor;

public class EditorApp : Game {

    public const int DefaultWorldSize = 8;
    public const int WindowRenderSizeX = 1920;
    public const int WindowRenderSizeY = 1080;

    private ScreenRenderer _renderer;
    private TabsManager _tabsManager;
    private World _world;

    private Texture2D _testTexture;

    public EditorApp() {
        _renderer = new ScaledScreenRenderer(this, Resolution.Small, Resolution.HD);
        IsMouseVisible = true;
        
        _tabsManager = new TabsManager([new SettingsTab(), new SceneTab(), new CharacterTab(), new AssetsTab()]);
        _world = new World(DefaultWorldSize, DefaultWorldSize);
        _renderer.OnRender += Render;
    }

    protected override void Initialize() {
        base.Initialize();
        _renderer.Init();
    }

    protected override void LoadContent() {
        _renderer.Load();
        using var fileStream = new FileStream("assets\\test_render.png", FileMode.Open);
            _testTexture = Texture2D.FromStream(GraphicsDevice, fileStream);
    }

    protected override void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        _renderer.Draw(gameTime);

        base.Draw(gameTime);
    }

    private void Render(SpriteBatch spriteBatch) {
        _tabsManager.Render(spriteBatch);
        spriteBatch.Draw(_testTexture, Vector2.Zero, Color.White);
    }

}
