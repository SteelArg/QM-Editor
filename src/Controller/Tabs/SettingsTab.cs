using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.File;
using QMEditor.View;

namespace QMEditor.Controllers;

public class SettingsTab : Tab {

    private WorldIOManagerView worldIOManagerView;

    private WorldSaver _worldSaver;
    private WorldLoader _worldLoader;

    public SettingsTab() : base() {
        worldIOManagerView = new WorldIOManagerView();
        _worldSaver = new WorldSaver();
        _worldLoader = new WorldLoader();
    }

    protected override Widget BuildUI() {
        worldIOManagerView.ClickedLoad += OnClickedLoad;
        worldIOManagerView.ClickedSave += OnClickedSave;
        return worldIOManagerView.BuildUI();
    }

    public override void Open() {}
    public override void Close() {}

    public void OnClickedLoad() {
        var fileDialog = new FileDialog(FileDialogMode.OpenFile) {
            Filter = "*.qmworld",
            FilePath = Path.GetFullPath("saves\\save.qmworld")
        };
        fileDialog.Closed += (s, a) => { if (fileDialog.Result) LoadWorld(fileDialog.FilePath); };
        fileDialog.Show(Global.Desktop);
    }

    public void OnClickedSave() {
        var fileDialog = new FileDialog(FileDialogMode.SaveFile) {
            Filter = "*.qmworld",
            FilePath = Path.GetFullPath("saves\\save.qmworld")
        };
        fileDialog.Closed += (s, a) => { if (fileDialog.Result) SaveWorld(fileDialog.FilePath); };
        fileDialog.Show(Global.Desktop);
    }

    public void SaveWorld(string path) {
        string saveName = Path.GetFileName(path);
        worldIOManagerView.SetSaveName(saveName);
        _worldSaver.Save(path);
    }

    public void LoadWorld(string path) {
        string saveName = Path.GetFileName(path);
        worldIOManagerView.SetSaveName(saveName);
        _worldLoader.Load(path);
    }
    
    public override void Draw(SpriteBatch spriteBatch) {
        // noop
    }
}