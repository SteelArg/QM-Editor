using Microsoft.Xna.Framework;
using QMEditor.Controllers;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor;

public class EditorApp : Game {

    private ScreenRenderer _renderer;
    private TabsManager _tabsManager;

    public EditorApp(bool launchSmall = false) {
        Global.SetGame(this);

        _renderer = new SeperatedScreenRenderer(Resolution.Pick(launchSmall ? Resolution.Small : Resolution.HD));
        IsMouseVisible = true;
        
        new World(WorldSettings.Default);
        new WorldEffectManager();
        new AppSettings();
        new Input();
        new FontLoader();

        _tabsManager = new TabsManager([new SettingsTab(), new SceneTab(), new CharacterTab(), new AssetsTab()]);

        ServiceLocator.LoggerService.Log("Application started.");
    }

    protected override void Initialize() {
        base.Initialize();
        _renderer.Init();
        _renderer.UIRenderList.AddRenderer(new TabsUIRenderer(_tabsManager));
        _renderer.SpriteRenderList.AddRenderer(new TabsSpriteRenderer(_tabsManager));
    }

    protected override void LoadContent() {
        FontLoader.Instance.Load();
        _renderer.Load();
        _tabsManager.Load();

        var defaultTile = new Tile(AssetsLoader.Instance.GetAsset("default", AssetsFolders.Tiles) ?? AssetsLoader.Instance.GetAnyAsset(AssetsFolders.Tiles));
        var tileFactory = new TileFactory(defaultTile);
        tileFactory.FillGrid(World.Instance.Grid);
    }

    protected override void Update(GameTime gameTime) {
        base.Update(gameTime);

        if (!IsActive) return;

        _tabsManager.Update(gameTime);

        Input.LateUpdate();
    }

    protected override void Draw(GameTime gameTime) {
        _renderer.Draw(gameTime);

        base.Draw(gameTime);
    }

}
