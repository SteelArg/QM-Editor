using System;
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
        DebugTimer debugTimer = new DebugTimer();

        Grid grid = World.Instance.Grid;
        int[] cursorPos = WorldEditor.CursorGridPosition;
        GridObjectRenderData defaultRenderData = new GridObjectRenderData(RenderSettings, totalDepth, frame);

        _cellRenderCommands = new Dictionary<(int, int), CellRenderCommands>();

        LoopThroughPositions.Every((x, y) => {
            float tileDepth = x + y;
            bool hovered = cursorPos != null && cursorPos[0] == x && cursorPos[1] == y && displayEditor;
            GridCell cell = grid.GetGridCell([x,y]);

            GridObjectRenderData cellRenderData = defaultRenderData.WithAddedDepth(tileDepth) with { 
                IsHovered = hovered, CellLift = cell.Tile?.GetLift(RenderSettings) ?? 0
            };

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
        // PASS 0: Default
        // PASS 1: Silhouettes
        for (int pass = 0; pass < 2; pass++) {
            ExecuteRenderCommandsPass(spriteBatch, pass);
        }
    }

    private void ExecuteRenderCommandsPass(SpriteBatch spriteBatch, int pass) {
        Grid grid = World.Instance.Grid;

        for (int layer = 0; layer < grid.Size[0]+grid.Size[1]-1; layer++) {
            for (int x = 0; x < layer+1; x++) {
                int y = layer-x;
                ExecuteGridCellRenderCommand(x, y, true, spriteBatch, pass);
            }
        }
    }

    private void ExecuteGridCellRenderCommand(int x, int y, bool recursive, SpriteBatch spriteBatch, int pass) {
        if (!_cellRenderCommands.ContainsKey((x,y))) return;

        CellRenderCommands cell = _cellRenderCommands[(x,y)];
        if (pass == 0 && cell.RenderedContents) return;

        if (pass != 0) {
            cell.RenderContents(spriteBatch, pass);
            return;
        }

        if (_cellRenderCommands.TryGetValue((x-1,y), out CellRenderCommands nCell)) {
            if (!nCell.PreparingToRenderContents) ExecuteGridCellRenderCommand(x-1, y, true, spriteBatch, pass);
        }
        if (_cellRenderCommands.TryGetValue((x,y-1), out CellRenderCommands n2Cell)) {
            if (!n2Cell.PreparingToRenderContents) ExecuteGridCellRenderCommand(x, y-1, true, spriteBatch, pass);
        }

        cell.RenderTile(spriteBatch);

        if (!recursive) return;


        cell.PrepareToRenderContents();
        if (_cellRenderCommands.TryGetValue((x,y+1), out CellRenderCommands dCell)) {
            if (!dCell.RenderedContents && dCell.Lift <= cell.Lift) ExecuteGridCellRenderCommand(x, y+1, false, spriteBatch, pass);
        }
        if (_cellRenderCommands.TryGetValue((x+1,y), out CellRenderCommands d2Cell)) {
            if (!d2Cell.RenderedContents && d2Cell.Lift <= cell.Lift) ExecuteGridCellRenderCommand(x+1, y, false, spriteBatch, pass);
        }
        
        cell.RenderContents(spriteBatch, pass);
    }

    public void SaveToGif(string path, int[] renderSize, int upscaling) {
        var spriteBatch = new SpriteBatch(Global.Game.GraphicsDevice);
        RenderTarget2D[] saveRTs = new RenderTarget2D[AppSettings.RenderFrameCount.Get()];

        for (int i = 0; i < AppSettings.RenderFrameCount.Get(); i++) {
            saveRTs[i] = RenderToTarget(spriteBatch, renderSize, i, upscaling);
        }
        
        ServiceLocator.FileService.SaveAsGif(path, saveRTs, renderSize, AppSettings.RenderFrameDuration.Get());
        ServiceLocator.MessageWindowsService.InfoWindow($"Finished rendering.\nGIF saved to {path}.");
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

    private class CellRenderCommands {
        
        public readonly int Lift;
        public bool RenderedTile { get => _renderedTile; }
        public bool RenderedContents { get => _renderedContents; }
        public bool PreparingToRenderContents { get => _preparingToRenderContents; }

        private readonly RenderCommandBase TileRender;
        private readonly RenderCommandBase ContentsRender;

        private bool _renderedTile;
        private bool _renderedContents;
        private bool _preparingToRenderContents;

        public CellRenderCommands(RenderCommandBase tileRenderCommand, RenderCommandBase[] contentsRenderCommands, int lift) {
            TileRender = tileRenderCommand;
            ContentsRender = new GroupedRenderCommand(contentsRenderCommands);
            Lift = lift;
        }

        public void RenderTile(SpriteBatch spriteBatch) {
            TileRender?.Execute(spriteBatch);
            _renderedTile = true;
        }

        public void RenderContents(SpriteBatch spriteBatch, int pass) {
            ContentsRender.Execute(spriteBatch, pass);
            _renderedContents = true;
        }

        public void PrepareToRenderContents() => _preparingToRenderContents = true;

    }

}