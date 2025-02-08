using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor.Controllers;

public class SceneTab : Tab {

    private WorldEditor _worldEditor;
    private WorldRenderer _worldRenderer;
    private FrameLooper _frameLooper;
    private SceneTabView _sceneTabView;

    private EditCommandsStack _editStack;
    private List<EditKeybind> _editKeybinds;

    public SceneTab() : base() {
        _worldEditor = new WorldEditor();
        _worldRenderer = new WorldRenderer();
        _frameLooper = FrameLooper.FromAppSettings();
        _sceneTabView = new SceneTabView();
        _editStack = new EditCommandsStack();
        _editKeybinds = new List<EditKeybind> {
            new EditKeybind(()=>Input.MouseButtonClicked(0), ()=>new PlaceGridObjectCommand(_worldEditor.GetEditContext(), World.Cursor.GetCopyOfObject())),
            new EditKeybind(()=>Input.MouseButtonClicked(1), ()=>new ClearGridCellCommand(_worldEditor.GetEditContext(), Input.KeyHeld(Keys.LeftShift))),
            new EditKeybind(()=>Input.MouseButtonClicked(2), ()=>new CopyGridObjectCommand(_worldEditor.GetEditContext(), Input.KeyHeld(Keys.LeftShift)))
        };
    }

    protected override Widget BuildUI() {
        Widget widget = _sceneTabView.BuildUI();
        _sceneTabView.RenderClicked += () => { _worldRenderer.SaveToGif("output\\render.gif", AppSettings.RenderOutputSize.Value.ToIntArray(), AppSettings.RenderOutputUpscaling.Value); };
        _sceneTabView.RenderSettingsClicked += OpenRenderSettingsDialog;
        _sceneTabView.FrameLooper.TogglePauseClicked += _frameLooper.TogglePause;
        _sceneTabView.FrameLooper.NextFrameClicked += _frameLooper.NextFrame;
        _sceneTabView.FrameLooper.PrevFrameClicked += _frameLooper.PrevFrame;
        _frameLooper.FrameChanged += _sceneTabView.FrameLooper.SetCurrentFrame;
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
            AppSettings.RenderOffset.SetValue(renderSettingsWidget.GetRenderOffset());
            AppSettings.RenderFrameDuration.SetValue(renderSettingsWidget.GetFrameDuration());
            AppSettings.RenderFrameCount.SetValue(renderSettingsWidget.GetFrameCount());
            AppSettings.RenderOutputSize.SetValue(renderSettingsWidget.GetOutputSize());
            AppSettings.RenderOutputUpscaling.SetValue(renderSettingsWidget.GetOutputUpscaling());
            AppSettings.Instance.Save();
            _worldRenderer.UpdateRenderSettings();
            _frameLooper = FrameLooper.FromAppSettings();
            _sceneTabView.FrameLooper.TogglePauseClicked += _frameLooper.TogglePause;
            _sceneTabView.FrameLooper.NextFrameClicked += _frameLooper.NextFrame;
            _sceneTabView.FrameLooper.PrevFrameClicked += _frameLooper.PrevFrame;
            _frameLooper.FrameChanged += _sceneTabView.FrameLooper.SetCurrentFrame;
        };
        dialog.ShowModal(Global.Desktop);
    }

    public override void Open() {}
    public override void Close() {}

    public override void Update(GameTime gameTime) {
        _frameLooper.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

        foreach (EditKeybind keybind in _editKeybinds) {
            _editStack.AddAndExecuteCommand(keybind.CreateCommandIfKeybindFired());
        }
        
        if (Input.KeyFired(Keys.Z) && Input.KeyHeld(Keys.LeftControl))
            _editStack.UndoLastCommand();
    }

    public override void Draw(SpriteBatch spriteBatch) {
        _worldRenderer.Render(spriteBatch, 1f, _frameLooper.CurrentFrame);
        _worldEditor.Render(spriteBatch);
    }

    private class EditKeybind {

        private Func<bool> _keybind;
        private Func<IEditCommand> _commandFactory;

        public EditKeybind(Func<bool> keybind, Func<IEditCommand> commandFactory) {
            _keybind = keybind;
            _commandFactory = commandFactory;
        }

        public IEditCommand CreateCommandIfKeybindFired() {
            if (!_keybind.Invoke()) return null;
            return _commandFactory.Invoke();
        }

    }

}