using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class SingleRenderCommand : OneTimeRenderCommandBase {

    public readonly SpriteRenderData SpriteRenderData;
    private readonly float _depth;

    public SingleRenderCommand(SpriteRenderData spriteRenderData, int? pass = null) : base(pass) {
        SpriteRenderData = spriteRenderData;
        _depth = spriteRenderData.Depth;
    }

    protected override void OnOneTimeRender(SpriteBatch spriteBatch) {
        spriteBatch.Draw(SpriteRenderData.Texture, SpriteRenderData.Position, null, SpriteRenderData.Color, 0f, Vector2.Zero, 1f, SpriteRenderData.GetSpriteEffects(), 0f);
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