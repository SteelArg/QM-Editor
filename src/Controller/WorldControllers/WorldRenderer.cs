using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldRenderer {

    public static GridRenderSettings RenderSettings { get; private set; }

    private Dictionary<(int, int), CellRenderCommands> _cellRenderCommands;

    public WorldRenderer() {
        UpdateRenderSettings();
    }

    public void Render(SpriteBatch spriteBatch, float totalDepth, int frame = 0, bool displayEditor = true) {
        Grid grid = World.Instance.Grid;
        int[] cursorPos = WorldEditor.CursorGridPosition;
        GridObjectRenderData defaultRenderData = new GridObjectRenderData(RenderSettings, totalDepth, frame);

        _cellRenderCommands = new Dictionary<(int, int), CellRenderCommands>();

        LoopThroughPositions.Every((x, y) => {
            float tileDepth = x + y;
            bool hovered = cursorPos != null && cursorPos[0] == x && cursorPos[1] == y && displayEditor;
            GridCell cell = grid.GetGridCell([x,y]);

            GridObjectRenderData cellRenderData = defaultRenderData.WithAddedDepth(tileDepth);
            cellRenderData.IsHovered = hovered;
            cellRenderData.CellLift = cell.Tile?.GetLift(RenderSettings) ?? 0;

            List<RenderCommandBase> contentsRenderCommands = new List<RenderCommandBase>();
            RenderCommandBase tileRenderCommand = null;

            foreach (GridObject obj in cell.Objects) {
                if (obj is Tile)
                    tileRenderCommand = obj.GetRenderCommand(cellRenderData);
                else
                    contentsRenderCommands.Add(obj.GetRenderCommand(cellRenderData));
            }

            _cellRenderCommands.Add((x,y), new CellRenderCommands(tileRenderCommand, contentsRenderCommands.ToArray(), cellRenderData.CellLift));
        }, grid.Size);

        // Execute render commands
        // PASS 0: Tiles
        // PASS 1: Characters and contets
        // PAss 2: Silhouettes
        for (int layer = 0; layer < grid.Size[0]+grid.Size[1]-1; layer++) {
            for (int x = 0; x < layer+1; x++) {
                int y = layer-x;
                if (!_cellRenderCommands.ContainsKey((x,y))) continue;

                CellRenderCommands cell = _cellRenderCommands[(x,y)];
                cell.RenderTile(spriteBatch);

                if (_cellRenderCommands.ContainsKey((x+1,y)) && _cellRenderCommands[(x+1,y)].Lift <= cell.Lift) {
                    if (_cellRenderCommands.ContainsKey((x+1,y-1)))
                        _cellRenderCommands[(x+1,y-1)].RenderTile(spriteBatch);
                    _cellRenderCommands[(x+1,y)].RenderTile(spriteBatch);
                }
                if (_cellRenderCommands.ContainsKey((x,y+1)) && _cellRenderCommands[(x,y+1)].Lift <= cell.Lift) {
                    if (_cellRenderCommands.ContainsKey((x-1,y+1)))
                        _cellRenderCommands[(x-1,y+1)].RenderTile(spriteBatch);
                    _cellRenderCommands[(x,y+1)].RenderTile(spriteBatch);
                }
                
                cell.RenderContents(spriteBatch);
            }
        }
    }

    public void SaveToGif(string path, int[] renderSize, int upscaling) {
        var spriteBatch = new SpriteBatch(Global.Game.GraphicsDevice);
        RenderTarget2D[] saveRTs = new RenderTarget2D[AppSettings.RenderFrameCount.Value];

        for (int i = 0; i < AppSettings.RenderFrameCount.Value; i++) {
            saveRTs[i] = RenderToTarget(spriteBatch, renderSize, i, upscaling);
        }
        
        ServiceLocator.FileService.SaveAsGif(path, saveRTs, renderSize, AppSettings.RenderFrameDuration.Value);
    }

    public void UpdateRenderSettings() {
        RenderSettings = GridRenderSettings.FromAppSettings();
    }

    private RenderTarget2D RenderToTarget(SpriteBatch spriteBatch, int[] renderSize, int frame, float scale) {
        var saveRT = new RenderTarget2D(Global.Game.GraphicsDevice, renderSize[0], renderSize[1]);
        
        Global.Game.GraphicsDevice.SetRenderTarget(saveRT);
        Global.Game.GraphicsDevice.Clear(Color.Transparent);
        
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(scale));
        Render(spriteBatch, 0f, frame, false);
        spriteBatch.End();

        Global.Game.GraphicsDevice.SetRenderTarget(null);
        return saveRT;
    }

    private struct CellRenderCommands {
        
        public readonly int Lift;

        private readonly RenderCommandBase TileRender;
        private readonly RenderCommandBase ContentsRender;

        public CellRenderCommands(RenderCommandBase tileRenderCommand, RenderCommandBase[] contentsRenderCommands, int lift) {
            TileRender = tileRenderCommand;
            ContentsRender = new GroupedRenderCommand(contentsRenderCommands);
            Lift = lift;
        }

        public void RenderTile(SpriteBatch spriteBatch) {
            TileRender?.Execute(spriteBatch);
        }

        public void RenderContents(SpriteBatch spriteBatch) {
            ContentsRender.Execute(spriteBatch);
        }

    }

}