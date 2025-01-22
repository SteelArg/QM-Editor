using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldRenderer {

    public void Render(SpriteBatch spriteBatch, float totalDepth, bool displayEditor = true) {
        Grid grid = World.Instance.Grid;
        var renderSettings = GridRenderSettings.Default;
        int[] cursorPos = WorldEditor.CursorPositionOnGrid;
        
        LoopThroughPositions.Every((x, y) => {
            float tileDepth = x + y;
            bool hovered = cursorPos != null && cursorPos[0] == x && cursorPos[1] == y && displayEditor;

            foreach (GridObject obj in grid.GetGridCell([x,y]).Objects) {
                obj.Render(spriteBatch, renderSettings, totalDepth + tileDepth, hovered);
            }
        }, grid.Size);
    }

    public void SaveToPng(string path, int[] renderSize) {
        var saveRT = new RenderTarget2D(Global.Game.GraphicsDevice, renderSize[0], renderSize[1]);
        var spriteBatch = new SpriteBatch(Global.Game.GraphicsDevice);
        
        Global.Game.GraphicsDevice.SetRenderTarget(saveRT);
        Global.Game.GraphicsDevice.Clear(Color.Transparent);
        
        spriteBatch.Begin(SpriteSortMode.FrontToBack);
        Render(spriteBatch, 0f, false);
        spriteBatch.End();

        Global.Game.GraphicsDevice.SetRenderTarget(null);
        
        ServiceLocator.FileService.SaveAsPng(path, saveRT, renderSize);
    }

}