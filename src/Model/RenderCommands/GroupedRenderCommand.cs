using System;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class GroupedRenderCommand : RenderCommand {

    public readonly RenderCommand[] RenderCommands;
    private readonly float _depth;

    public GroupedRenderCommand(RenderCommand[] spritesRenderData) {
        RenderCommands = spritesRenderData;
        
        float totalDepth = RenderCommands[0].Depth;
        foreach (RenderCommand renderCommand in RenderCommands) {
            totalDepth = Math.Min(renderCommand.Depth, totalDepth);
        }
        _depth = totalDepth;

        Span<RenderCommand> commandsSpan = new Span<RenderCommand>(RenderCommands);
        commandsSpan.Sort();
    }

    public override void Execute(SpriteBatch spriteBatch) {
        foreach (RenderCommand renderCommand in RenderCommands) {
            renderCommand.Execute(spriteBatch);
        }
    }

    protected override float GetDepth() => _depth;

}