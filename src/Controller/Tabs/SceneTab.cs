using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor.Controllers;

public partial class SceneTab : Tab {

    private WorldEditor _worldEditor;
    private WorldRenderer _worldRenderer;
    private FrameLooper _frameLooper;
    private SceneTabView _sceneTabView;
    private Inspector _inspector;

    private EditCommandsStack _editStack;
    private IEditKeybindsGenerator _editKeybindsGenerator;
    private List<EditKeybind> _editKeybinds;

    public SceneTab() : base() {
        _worldEditor = new WorldEditor();
        _worldRenderer = new WorldRenderer();
        _frameLooper = FrameLooper.FromAppSettings();
        _sceneTabView = new SceneTabView();
        _inspector = new Inspector();

        _editStack = new EditCommandsStack();
        _editKeybindsGenerator = new DefaultEditKeybindsGenerator(_worldEditor);
        _editKeybinds = _editKeybindsGenerator.Generate();
    }

    protected override Widget BuildUI() {
        Widget widget = _sceneTabView.BuildUI(_inspector.BuildUI());

        _sceneTabView.RenderClicked += () => { _worldRenderer.SaveToGif("output\\render.gif", AppSettings.RenderOutputSize.Get(), AppSettings.RenderOutputUpscaling.Get()); };
        _sceneTabView.RenderSettingsClicked += OpenRenderSettingsDialog;
        SetEventsForFrameLooper();

        return widget;
    }

    public void OpenRenderSettingsDialog() {
        var renderSettingsWidget = new RenderSettingsWidget();
        var dialog = new Dialog {
            Title = "Render Settings",
            Content = renderSettingsWidget
        };

        dialog.Closed += (s, e) => {
            if (!dialog.Result) return;
            renderSettingsWidget.WriteToAppSettings();
            _worldRenderer.UpdateRenderSettings();
            _frameLooper = FrameLooper.FromAppSettings();
            SetEventsForFrameLooper();
        };
        
        dialog.ShowModal(Global.Desktop);
    }

    private void SetEventsForFrameLooper() {
        _sceneTabView.FrameLooper.TogglePauseClicked += _frameLooper.TogglePause;
        _sceneTabView.FrameLooper.NextFrameClicked += _frameLooper.NextFrame;
        _sceneTabView.FrameLooper.PrevFrameClicked += _frameLooper.PrevFrame;
        _frameLooper.FrameChanged += _sceneTabView.FrameLooper.SetCurrentFrame;
    }

    public override void Open() {}
    public override void Close() {}

    public override void Update(GameTime gameTime) {
        _frameLooper.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

        EditContext ctx = _worldEditor.GetEditContext();

        foreach (EditKeybind keybind in _editKeybinds) {
            _editStack.AddAndExecuteCommand(keybind.CreateCommandIfKeybindFired());
        }
        
        if (Input.KeyFired(Keys.Z) && Input.KeyHeld(Keys.LeftControl))
            _editStack.UndoLastCommand();

        if (Input.MouseButtonClicked(0) && World.Cursor.IsEmpty && ctx.CursorGridPosition != null) {
            GridCell cell = World.Instance.Grid.GetGridCell(ctx.CursorGridPosition);
            _inspector.Inspect(cell);
        }
    }

    public override void Draw(SpriteBatch spriteBatch) {
        _worldRenderer.Render(spriteBatch, 1f, _frameLooper.CurrentFrame);
        _worldEditor.Render(spriteBatch);
    }

}