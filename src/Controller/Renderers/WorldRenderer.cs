using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public static class WorldRenderer {

    public static void Render(SpriteBatch spriteBatch, float totalDepth) {
        Grid grid = World.Instance.Grid;
        var renderSettings = GridRenderSettings.Default;
        
        LoopThroughPositions.Every((x, y) => {
            float tileDepth = x + y;
            foreach (GridObject obj in grid.GetObjectsOnGridPosition(new Vector2(x, y))) {
                obj.Render(spriteBatch, renderSettings, totalDepth + tileDepth);
            }
        }, grid.Size);
    }

}