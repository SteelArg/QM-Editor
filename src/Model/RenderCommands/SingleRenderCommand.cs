using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class SingleRenderCommand : RenderCommand {

    public readonly SpriteRenderData SpriteRenderData;
    private readonly float _depth;

    public SingleRenderCommand(SpriteRenderData spriteRenderData) {
        SpriteRenderData = spriteRenderData;
        _depth = spriteRenderData.Depth;
    }

    public override void Execute(SpriteBatch spriteBatch) {
        spriteBatch.Draw(SpriteRenderData.Texture, SpriteRenderData.Position, null, SpriteRenderData.Color);
    }

    protected override float GetDepth() => _depth;

}

public struct SpriteRenderData : IComparable<SpriteRenderData> {
    
    public Texture2D Texture;
    public Vector2 Position;
    public Color Color;
    public float Depth;

    public int CompareTo(SpriteRenderData other) {
        return Depth.CompareTo(other.Depth);
    }

}