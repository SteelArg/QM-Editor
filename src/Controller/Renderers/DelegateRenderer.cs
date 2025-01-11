using System;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Controllers;

public class DelegateRenderer : Renderer {

    private Action<SpriteBatch> _render;

    public DelegateRenderer(Action<SpriteBatch> render) {
        _render = render;
    }

    public override void Render(SpriteBatch spriteBatch) {
        _render(spriteBatch);
    }

}