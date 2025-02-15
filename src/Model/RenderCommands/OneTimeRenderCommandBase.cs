using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public abstract class OneTimeRenderCommandBase : RenderCommandBase {

    private bool _wasExecuted = false;

    public OneTimeRenderCommandBase(int? pass = null) : base(pass) {}

    protected override void OnRender(SpriteBatch spriteBatch, int pass = 0) {
        if (_wasExecuted) return;
        OnOneTimeRender(spriteBatch);
        _wasExecuted = true;
    }

    protected abstract void OnOneTimeRender(SpriteBatch spriteBatch);

}