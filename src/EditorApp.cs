using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using QMEditor.Controllers;
using QMEditor.Model;

namespace QMEditor;

public class EditorApp : Game {

    private ScreenRenderer _renderer;
    private TabsManager _tabsManager;

    public EditorApp() {
        Global.SetGame(this);

        _renderer = new SeperatedScreenRenderer(Resolution.Pick(Resolution.HD), 4f);
        IsMouseVisible = true;
        
        new World(WorldSettings.Default);
        new AppSettings();

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
        
        World.Instance.Grid.PlaceOnGrid(AssetsLoader.Instance.GetCharacter("steven"), new Vector2(2, 3));

        var tileFactory = new TileFactory(AssetsLoader.Instance.GetAsset("default", AssetsFolders.Tiles));
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

    public new void Exit() {
        AppSettings.Instance.Save();
        WorldSaver.Save();
        base.Exit();
    }

}
