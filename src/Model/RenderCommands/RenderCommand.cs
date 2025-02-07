using System;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public abstract class RenderCommandBase : IComparable<RenderCommandBase> {

    public float Depth { get => GetDepth(); }

    public int CompareTo(RenderCommandBase other) {
        return Depth.CompareTo(other.Depth);
    }

    public abstract void Execute(SpriteBatch spriteBatch);

    protected abstract float GetDepth();

}

public class EmptyRenderCommand : RenderCommandBase {

    public EmptyRenderCommand() {}

    public override void Execute(SpriteBatch spriteBatch) {
        // noop
    }

    protected override float GetDepth() => 0f;

}