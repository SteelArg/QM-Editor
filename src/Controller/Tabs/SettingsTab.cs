using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.File;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor.Controllers;

public class SettingsTab : Tab {

    private WorldIOManagerView worldIOManagerView;

    private WorldSaver _worldSaver;
    private WorldLoader _worldLoader;

    private string _currentSavePath = null;

    public SettingsTab() : base() {
        worldIOManagerView = new WorldIOManagerView();
        _worldSaver = new WorldSaver();
        _worldLoader = new WorldLoader();
    }

    protected override Widget BuildUI() {
        worldIOManagerView.ClickedLoad += () => OpenWorldDialog(FileDialogMode.OpenFile);
        worldIOManagerView.ClickedSave += () => OpenWorldDialog(FileDialogMode.SaveFile);
        worldIOManagerView.ClickedNew += OnClickedNew;
        return worldIOManagerView.BuildUI();
    }

    public override void Open() {}
    public override void Close() {}

    public void OpenWorldDialog(FileDialogMode fileMode) {
        var fileDialog = new FileDialog(fileMode) {
            Filter = "*.qmworld",
            FilePath = _currentSavePath ?? Path.GetFullPath("saves\\save.qmworld")
        };
        fileDialog.Closed += (s, a) => {
            if (fileDialog.Result)
                SaveOrLoadWorld(fileDialog.FilePath, fileMode);
        };
        fileDialog.ShowModal(Global.Desktop);
    }

    public void OnClickedNew() {
        var newWorldWidget = new NewWorldBuilderWidget();
        var newWorldDialog = new Dialog {
            Title = "Create New World",
            Content = newWorldWidget
        };
        newWorldDialog.Closed += (s, a) => { if (newWorldDialog.Result) NewWorld(newWorldWidget.GetWorldSize(), newWorldWidget.GetFillGrid()); };
        newWorldDialog.ShowModal(Global.Desktop);
    }

    public void SaveOrLoadWorld(string path, FileDialogMode fileMode) {
        string saveName = Path.GetFileName(path);
        worldIOManagerView.SetSaveName(saveName);
        _currentSavePath = path;
        if (fileMode == FileDialogMode.OpenFile)
            _worldLoader.Load(path);
        else if (fileMode == FileDialogMode.SaveFile)
            _worldSaver.Save(path);
    }

    public void NewWorld(int[] size, bool fillGrid) {
        worldIOManagerView.SetSaveName("");
        _currentSavePath = null;
        
        new World(new WorldSettings(size));
        WorldEffectManager.ClearEffect();

        // Fill Grid
        if (!fillGrid) return;
        AssetBase defaultTileAsset = AssetsLoader.Instance.GetAsset("default", AssetsFolders.Tiles);
        defaultTileAsset = defaultTileAsset ?? AssetsLoader.Instance.GetAnyAsset(AssetsFolders.Tiles);
        var defaultTileFactory = new TileFactory(defaultTileAsset);
        defaultTileFactory.FillGrid(World.Instance.Grid);
    }
    
    public override void Draw(SpriteBatch spriteBatch) {
        // noop
    }
}