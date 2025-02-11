using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class SingleRenderCommand : RenderCommandBase {

    public readonly SpriteRenderData SpriteRenderData;
    private readonly float _depth;

    private bool _wasExecuted;

    public SingleRenderCommand(SpriteRenderData spriteRenderData) {
        SpriteRenderData = spriteRenderData;
        _depth = spriteRenderData.Depth;
    }

    public override void Execute(SpriteBatch spriteBatch) {
        if (_wasExecuted) return;
        spriteBatch.Draw(SpriteRenderData.Texture, SpriteRenderData.Position, null, SpriteRenderData.Color, 0f, Vector2.Zero, 1f, SpriteRenderData.GetSpriteEffects(), 0f);
        _wasExecuted = true;
    }

    protected override float GetDepth() => _depth;

}

public struct SpriteRenderData : IComparable<SpriteRenderData> {
    
    public Texture2D Texture;
    public Vector2 Position;
    public Color Color;
    public float Depth;
    public bool Flip;

    public int CompareTo(SpriteRenderData other) {
        return Depth.CompareTo(other.Depth);
    }

    public SpriteEffects GetSpriteEffects() {
        return Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    }

}