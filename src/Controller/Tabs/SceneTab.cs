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

    public SceneTab() : base() {
        _worldEditor = new WorldEditor();
        _worldRenderer = new WorldRenderer();
        _frameLooper = new FrameLooper(AppSettings.RenderFrameCount.Value, AppSettings.RenderFrameDuration.Value/1000f);
        _sceneTabView = new SceneTabView();
    }

    protected override Widget BuildUI() {
        _sceneTabView.RenderClicked += () => { _worldRenderer.SaveToGif("output\\render.gif", [720, 480]); };
        _sceneTabView.FrameLooperView.TogglePauseClicked += _frameLooper.TogglePause;
        _sceneTabView.FrameLooperView.NextFrameClicked += _frameLooper.NextFrame;
        _sceneTabView.FrameLooperView.PrevFrameClicked += _frameLooper.PrevFrame;
        _frameLooper.FrameChanged += _sceneTabView.FrameLooperView.SetCurrentFrame;
        return _sceneTabView.BuildUI();
    }

    public override void Open() {}
    public override void Close() {}

    public override void Update(GameTime gameTime) {
        _frameLooper.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

        if (Input.MouseButtonClicked(0))
            _worldEditor.PlaceObjectOnCursor();
        if (Input.MouseButtonHeld(1))
            _worldEditor.ClearCellOnCursor(Input.KeyHeld(Keys.LeftShift));
        if (Input.MouseButtonClicked(2))
            _worldEditor.CopyGridObjectOnCursor(Input.KeyHeld(Keys.LeftShift));
    }

    public override void Draw(SpriteBatch spriteBatch) {
        _worldRenderer.Render(spriteBatch, 1f, _frameLooper.CurrentFrame);
        _worldEditor.Render(spriteBatch);
    }
}