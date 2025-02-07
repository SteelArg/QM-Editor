using System;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class GroupedRenderCommand : RenderCommandBase {

    public readonly RenderCommandBase[] RenderCommands;
    private readonly float _depth;

    public GroupedRenderCommand(RenderCommandBase[] spritesRenderData) {
        RenderCommands = spritesRenderData;
        if (RenderCommands.Length < 1) return;
        
        float totalDepth = RenderCommands[0].Depth;
        foreach (RenderCommandBase renderCommand in RenderCommands) {
            totalDepth = Math.Min(renderCommand.Depth, totalDepth);
        }
        _depth = totalDepth;

        Span<RenderCommandBase> commandsSpan = new Span<RenderCommandBase>(RenderCommands);
        commandsSpan.Sort();
    }

    public override void Execute(SpriteBatch spriteBatch) {
        foreach (RenderCommandBase renderCommand in RenderCommands) {
            renderCommand.Execute(spriteBatch);
        }
    }

    protected override float GetDepth() => _depth;

}