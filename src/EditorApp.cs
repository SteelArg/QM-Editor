using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QMEditor.Controllers;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor;

public class EditorApp : Game {

    private ScreenRenderer _renderer;
    private TabsManager _tabsManager;

    private Texture2D _testTexture;

    public EditorApp(bool launchSmall = false) {
        Global.SetGame(this);

        _renderer = new SeperatedScreenRenderer(Resolution.Pick(launchSmall ? Resolution.Small : Resolution.HD));
        IsMouseVisible = true;
        
        new World(WorldSettings.Default);
        new WorldEffectManager();
        new AppSettings();
        new Input();

        _tabsManager = new TabsManager([new SettingsTab(), new SceneTab(), new CharacterTab(), new AssetsTab()]);
    }

    protected override void Initialize() {
        base.Initialize();
        _renderer.Init();
        _renderer.UIRenderList.AddRenderer(new TabsUIRenderer(_tabsManager));
        _renderer.SpriteRenderList.AddRenderer(new TabsSpriteRenderer(_tabsManager));
        // _renderer.SpriteRenderList.AddRenderer(new DelegateRenderer( (sb) => { if (_testTexture == null) return; sb.Draw(_testTexture, Vector2.Zero, Color.White); }));
    }

    protected override void LoadContent() {
        _renderer.Load();
        _tabsManager.Load();

        var defaultTile = new Tile(AssetsLoader.Instance.GetAsset("default", AssetsFolders.Tiles) ?? AssetsLoader.Instance.GetAnyAsset(AssetsFolders.Tiles));
        var tileFactory = new TileFactory(defaultTile);
        tileFactory.FillGrid(World.Instance.Grid);
        _testTexture = defaultTile.Asset.GetTexture();
    }

    protected override void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _tabsManager.Update(gameTime);

        base.Update(gameTime);

        Input.LateUpdate();
    }

    protected override void Draw(GameTime gameTime) {
        _renderer.Draw(gameTime);

        base.Draw(gameTime);
    }

    public new void Exit() {
        AppSettings.Instance.Save();
        base.Exit();
    }

}
