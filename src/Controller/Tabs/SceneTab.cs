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
        _inspector.ExecuteCommand += AddAndExecuteEditCommand;

        _editStack = new EditCommandsStack();
        _editKeybindsGenerator = new DefaultEditKeybindsGenerator();
        _editKeybinds = _editKeybindsGenerator.Generate();
    }

    public override void Load() {
        base.Load();
        _worldRenderer.Load();
    }

    protected override Widget BuildUI() {
        Widget widget = _sceneTabView.BuildUI(_inspector.BuildUI());

        _sceneTabView.RenderClicked += () => { _worldRenderer.SaveToGif("output\\render.gif"); };
        _sceneTabView.RenderSettingsClicked += OpenRenderSettingsDialog;

        _sceneTabView.ShaderSelection.ShaderSelected += (p) => WorldEffectManager.LoadEffect(p);
        _sceneTabView.ShaderSelection.ShaderUserVariableSelected += (userVariable) => { WorldEffectManager.CurrentEffect?.SetUserVariable(userVariable); };
        WorldEffectManager.EffectChanged += (WorldEffect we) => { _sceneTabView.ShaderSelection.SetEffectNameByPath(we?.GetShaderPath(), we?.GetUserVariableConstraints()); _sceneTabView.ShaderSelection.SetEffectUserVariable(we?.GetUserVariable() ?? 0f); };

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

        if (Global.Desktop.HasModalWidget) return;

        EditContext ctx = _worldEditor.GetEditContext();

        foreach (EditKeybind keybind in _editKeybinds) {
            _editStack.AddAndExecuteCommand(keybind.CreateCommandIfKeybindFired(ctx));
        }
        
        if (Input.KeyFired(Keys.Z) && Input.KeyHeld(Keys.LeftControl))
            _editStack.UndoLastCommand();

        if (Input.MouseButtonClicked(0) && World.Cursor.IsEmpty && ctx.CursorGridPosition != null) {
            GridCell cell = World.Instance.Grid.GetGridCell(ctx.CursorGridPosition);
            _inspector.Inspect(cell);
        }
    }

    public void AddAndExecuteEditCommand(EditCommandBase editCommand) => _editStack.AddAndExecuteCommand(editCommand);

    public void ClearEditCommandStack() => _editStack.Clear();

    public override RenderTarget2D Draw(SpriteBatch spriteBatch) {
        RenderTarget2D worldRT = _worldRenderer.RenderToTarget(_frameLooper.CurrentFrame, null, null, true);

        if (!Global.Desktop.HasModalWidget) {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(AppSettings.RenderOutputUpscaling.Get()));
            _worldEditor.Render(spriteBatch);
            spriteBatch.End();
        }
        
        return worldRT;
    }

}