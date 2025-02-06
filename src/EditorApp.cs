using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using QMEditor.Controllers;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor;

public class EditorApp : Game {

    private ScreenRenderer _renderer;
    private TabsManager _tabsManager;

    public EditorApp() {
        Global.SetGame(this);

        _renderer = new SeperatedScreenRenderer(Resolution.Pick(Resolution.HD), 6f);
        IsMouseVisible = true;
        
        new World(WorldSettings.Default);
        new AppSettings();
        new Input();

        _tabsManager = new TabsManager([new SettingsTab(), new SceneTab(), new CharacterTab(), new AssetsTab()]);
        _renderer.UIRenderList.AddRenderer(new TabsUIRenderer(_tabsManager));
        _renderer.SpriteRenderList.AddRenderer(new TabsSpriteRenderer(_tabsManager));
    }

    protected override void Initialize() {
        base.Initialize();
        _renderer.Init();
    }

    protected override void LoadContent() {
        _renderer.Load();
        _tabsManager.Load();
        
        AssetBase stevenAsset = AssetsLoader.Instance.GetAsset("steven", AssetsFolders.Characters);
        World.Instance.Grid.PlaceOnGrid(new Character(stevenAsset), [2,3]);

        var defaultTile = new Tile(AssetsLoader.Instance.GetAsset("default", AssetsFolders.Tiles));
        var tileFactory = new TileFactory(defaultTile);
        tileFactory.FillGrid(World.Instance.Grid);
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
