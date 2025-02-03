using System;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public abstract class RenderCommand : IComparable<RenderCommand> {

    public float Depth { get => GetDepth(); }

    public int CompareTo(RenderCommand other) {
        return Depth.CompareTo(other.Depth);
    }

    public abstract void Execute(SpriteBatch spriteBatch);

    protected abstract float GetDepth();

}

public class EmptyRenderCommand : RenderCommand {

    public EmptyRenderCommand() {}

    public override void Execute(SpriteBatch spriteBatch) {
        // noop
    }

    protected override float GetDepth() => 0f;

}