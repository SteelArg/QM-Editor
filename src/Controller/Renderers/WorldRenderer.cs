using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public static class WorldRenderer {

    public static void Render(SpriteBatch spriteBatch, float totalDepth, bool displayEditor = true) {
        Grid grid = World.Instance.Grid;
        var renderSettings = GridRenderSettings.Default;

        int[] cursorPos = ValidateGridPosition(renderSettings.ScreenPositionToGrid(ScreenRenderer.Instance.GetMousePosition()));
        
        LoopThroughPositions.Every((x, y) => {
            float tileDepth = x + y;
            bool hovered = cursorPos[0] == x && cursorPos[1] == y && displayEditor;

            foreach (GridObject obj in grid.GetObjectsOnGridPosition(new Vector2(x, y))) {
                obj.Render(spriteBatch, renderSettings, totalDepth + tileDepth, hovered);
            }
        }, grid.Size);
    }

    public static void SaveToPng(string path, int[] renderSize) {
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

    private static int[] ValidateGridPosition(int[] originalPos) {
        if (originalPos[0] < 0 || originalPos[0] >= World.Instance.Grid.Size.X) return [-1, -1];
        if (originalPos[1] < 0 || originalPos[1] >= World.Instance.Grid.Size.Y) return [-1, -1];
        return originalPos;
    }

}