using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldRenderer {

    private const int renderFrames = 6;

    public void Render(SpriteBatch spriteBatch, float totalDepth, int frame = 0, bool displayEditor = true) {
        Grid grid = World.Instance.Grid;
        var renderSettings = GridRenderSettings.Default;
        int[] cursorPos = WorldEditor.CursorPositionOnGrid;
        
        LoopThroughPositions.Every((x, y) => {
            float tileDepth = x + y;
            bool hovered = cursorPos != null && cursorPos[0] == x && cursorPos[1] == y && displayEditor;

            foreach (GridObject obj in grid.GetGridCell([x,y]).Objects) {
                obj.Render(spriteBatch, renderSettings, totalDepth + tileDepth, frame, hovered);
            }
        }, grid.Size);
    }

    public void SaveToGif(string path, int[] renderSize) {
        var spriteBatch = new SpriteBatch(Global.Game.GraphicsDevice);
        RenderTarget2D[] saveRTs = new RenderTarget2D[renderFrames];

        for (int i = 0; i < renderFrames; i++) {
            saveRTs[i] = RenderToTarget(spriteBatch, renderSize, i);
        }
        
        ServiceLocator.FileService.SaveAsGif(path, saveRTs, renderSize, 200);
    }

    private RenderTarget2D RenderToTarget(SpriteBatch spriteBatch, int[] renderSize, int frame) {
        var saveRT = new RenderTarget2D(Global.Game.GraphicsDevice, renderSize[0], renderSize[1]);
        
        Global.Game.GraphicsDevice.SetRenderTarget(saveRT);
        Global.Game.GraphicsDevice.Clear(Color.Transparent);
        
        spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
        Render(spriteBatch, 0f, frame, false);
        spriteBatch.End();

        Global.Game.GraphicsDevice.SetRenderTarget(null);
        return saveRT;
    }

}