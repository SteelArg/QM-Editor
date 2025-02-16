using System;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public abstract class RenderCommandBase : IComparable<RenderCommandBase> {

    public float Depth { get => GetDepth(); }

    private int? _pass;

    public RenderCommandBase(int? pass = null) {
        _pass = pass;
    }

    public int CompareTo(RenderCommandBase other) {
        return Depth.CompareTo(other.Depth);
    }

    public void Execute(SpriteBatch spriteBatch, int pass = 0) {
        if (pass != _pass && _pass.HasValue) return;
        OnRender(spriteBatch, pass);
    }

    public virtual void SetPass(int? pass) => _pass = pass;

    protected abstract void OnRender(SpriteBatch spriteBatch, int pass = 0);
    protected abstract float GetDepth();

}

public class EmptyRenderCommand : RenderCommandBase {

    public EmptyRenderCommand() {}

    protected override void OnRender(SpriteBatch spriteBatch = null, int pass = 0) {
        // noop
    }

    protected override float GetDepth() => 0f;

}