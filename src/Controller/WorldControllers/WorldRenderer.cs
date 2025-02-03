using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldRenderer {

    public static GridRenderSettings RenderSettings { get; private set; }

    public WorldRenderer() {
        RenderSettings = GridRenderSettings.FromAppSettings();
    }

    public void Render(SpriteBatch spriteBatch, float totalDepth, int frame = 0, bool displayEditor = true) {
        Grid grid = World.Instance.Grid;
        int[] cursorPos = WorldEditor.CursorPositionOnGrid;
        GridObjectRenderData defaultRenderData = new GridObjectRenderData(RenderSettings, totalDepth, frame);

        List<RenderCommand> renderCommands = new List<RenderCommand>();

        LoopThroughPositions.Every((x, y) => {
            float tileDepth = x + y;
            bool hovered = cursorPos != null && cursorPos[0] == x && cursorPos[1] == y && displayEditor;

            GridObjectRenderData cellRenderData = defaultRenderData.WithAddedDepth(tileDepth);
            cellRenderData.IsHovered = hovered;
            cellRenderData.CellLift = grid.GetGridCell([x,y]).Tile?.GetLift(RenderSettings) ?? 0;

            foreach (GridObject obj in grid.GetGridCell([x,y]).Objects) {
                renderCommands.Add(obj.GetRenderCommand(cellRenderData));
            }
        }, grid.Size);

        var worldRenderCommand = new GroupedRenderCommand(renderCommands.ToArray());
        worldRenderCommand.Execute(spriteBatch);
    }

    public void SaveToGif(string path, int[] renderSize) {
        var spriteBatch = new SpriteBatch(Global.Game.GraphicsDevice);
        RenderTarget2D[] saveRTs = new RenderTarget2D[AppSettings.RenderFrameCount.Value];

        for (int i = 0; i < AppSettings.RenderFrameCount.Value; i++) {
            saveRTs[i] = RenderToTarget(spriteBatch, renderSize, i);
        }
        
        ServiceLocator.FileService.SaveAsGif(path, saveRTs, renderSize, AppSettings.RenderFrameDuration.Value);
    }

    private RenderTarget2D RenderToTarget(SpriteBatch spriteBatch, int[] renderSize, int frame) {
        var saveRT = new RenderTarget2D(Global.Game.GraphicsDevice, renderSize[0], renderSize[1]);
        
        Global.Game.GraphicsDevice.SetRenderTarget(saveRT);
        Global.Game.GraphicsDevice.Clear(Color.Transparent);
        
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(4f));
        Render(spriteBatch, 0f, frame, false);
        spriteBatch.End();

        Global.Game.GraphicsDevice.SetRenderTarget(null);
        return saveRT;
    }

}