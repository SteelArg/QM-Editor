using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using QMEditor.Controllers;
using QMEditor.Model;

namespace QMEditor;

public class EditorApp : Game {

    private ScreenRenderer _renderer;
    private TabsManager _tabsManager;

    private Texture2D _testTexture;

    public EditorApp() {
        _renderer = new SeperatedScreenRenderer(this, Resolution.Pick(Resolution.HD), 4f);
        IsMouseVisible = true;
        
        new World(WorldSettings.Default);
        new AppSettings();
        _tabsManager = new TabsManager([new SettingsTab(), new SceneTab(), new CharacterTab(), new AssetsTab()]);
        _renderer.UIRenderList.AddRenderer(new TabsUIRenderer(_tabsManager));
        _renderer.SpriteRenderList.AddRenderer(new TabsSpriteRenderer(_tabsManager));
        //_renderer.SpriteRenderList.AddRenderer(new DelegateRenderer((sb) => {sb.Draw(_testTexture, Vector2.Zero, null, Color.White);}));
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
        
        // Steven character
        Asset stevenAsset = new Asset("test\\steven.png");
        stevenAsset.Load(this);
        var steven = new Character(stevenAsset);
        World.Instance.Grid.PlaceOnGrid(steven, new Vector2(2, 3));

        // Fill grid with tiles
        Asset tileAsset = new Asset("test\\tile.png");
        tileAsset.Load(this);
        var tileFactory = new TileFactory(tileAsset);
        tileFactory.FillGrid(World.Instance.Grid);
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
