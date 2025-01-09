using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using QMEditor.Controllers;

namespace QMEditor;

public class EditorApp : Game {

    private ScreenRenderer _renderer;
    private TabsManager _tabsManager;

    private Texture2D _testTexture;

    public EditorApp() {
        _renderer = new SeperatedScreenRenderer(this, Resolution.HD);
        IsMouseVisible = true;
        
        _tabsManager = new TabsManager([new SettingsTab(), new SceneTab(), new CharacterTab(), new AssetsTab()]);
        _renderer.UIRenderList.AddRenderer(new TabsUIRenderer(_tabsManager));
        _renderer.SpriteRenderList.AddRenderer(new TabsSpriteRenderer(_tabsManager));
        _renderer.SpriteRenderList.AddRenderer(new DelegateRenderer((sb) => {sb.Draw(_testTexture, Vector2.Zero, null, Color.White);}));
    }

    protected override void Initialize() {
        base.Initialize();
        _renderer.Init();
    }

    protected override void LoadContent() {
        MyraEnvironment.Game = this;
        _renderer.Load();
        _tabsManager.Load();
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

}
