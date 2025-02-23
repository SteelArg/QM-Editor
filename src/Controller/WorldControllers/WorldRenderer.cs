using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldRenderer : Singleton<WorldRenderer> {

    public static GridRenderSettings RenderSettings { get; private set; }

    private Dictionary<(int, int), CellRenderCommands> _cellRenderCommands;
    private GraphicsDevice _graphicsDevice;
    private SpriteBatch _spriteBatch;

    public WorldRenderer() : base() {
        UpdateRenderSettings();
    }

    public void Load() {
        // _graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, new PresentationParameters());
        _graphicsDevice = Global.Game.GraphicsDevice;
        _spriteBatch = new SpriteBatch(_graphicsDevice);
    }

    public void SaveToGif(string path, int[] renderSize = null, int? upscaling = null) {
        // Default params
        upscaling = upscaling ?? AppSettings.RenderOutputUpscaling.Get();
        renderSize = renderSize ?? AppSettings.RenderOutputSize.Get();

        RenderTarget2D[] saveRTs = new RenderTarget2D[AppSettings.RenderFrameCount.Get()];

        for (int i = 0; i < AppSettings.RenderFrameCount.Get(); i++) {
            saveRTs[i] = RenderToTarget(i, renderSize, upscaling);
        }
        
        ServiceLocator.FileService.SaveAsGif(path, saveRTs, renderSize, AppSettings.RenderFrameDuration.Get());
        ServiceLocator.MessageWindowsService.InfoWindow($"Finished rendering.\nGIF saved to {path}.");

        Global.Game.GraphicsDevice.SetRenderTarget(null);
    }

    public void UpdateRenderSettings() {
        RenderSettings = GridRenderSettings.FromAppSettings();
    }

    public RenderTarget2D RenderToTarget(int frame, int[] renderSize = null, float? scale = null, bool displayEditor = false) {
        // Default params
        scale = scale ?? AppSettings.RenderOutputUpscaling.Get();
        renderSize = renderSize ?? AppSettings.RenderOutputSize.Get();

        var renderRT = new RenderTarget2D(_graphicsDevice, renderSize[0], renderSize[1]);
        
        _graphicsDevice.SetRenderTarget(renderRT);
        _graphicsDevice.Clear(Color.Transparent);

        Render(0f, frame, scale.Value, displayEditor);
        
        return renderRT;
    }

    private void Render(float totalDepth, int frame = 0, float scale = 1f, bool displayEditor = true) {
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

        var renderBS = new BlendState();
        renderBS.ColorSourceBlend = Blend.SourceAlpha;
        renderBS.AlphaSourceBlend = Blend.One;
        renderBS.ColorDestinationBlend = Blend.InverseSourceAlpha;
        renderBS.AlphaDestinationBlend = Blend.InverseSourceAlpha;

        var preserveAlphaBS = new BlendState();
        preserveAlphaBS.ColorSourceBlend = Blend.SourceAlpha;
        preserveAlphaBS.AlphaSourceBlend = Blend.Zero;
        preserveAlphaBS.ColorDestinationBlend = Blend.InverseSourceAlpha;
        preserveAlphaBS.AlphaDestinationBlend = Blend.One;

        // Execute render commands
        // PASS 0: Default
        // PASS 1: Silhouettes
        // PASS 2: Post-Shader
        _spriteBatch.Begin(SpriteSortMode.Immediate, renderBS, SamplerState.PointClamp, null, null, WorldEffectManager.CurrentEffect, Matrix.CreateScale(scale));
        ExecuteRenderCommandsPass(0);
        ExecuteRenderCommandsPass(1);
        _spriteBatch.End();

        _spriteBatch.Begin(SpriteSortMode.Immediate, preserveAlphaBS, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(scale));
        ExecuteRenderCommandsPass(2);
        _spriteBatch.End();
    }

    private void ExecuteRenderCommandsPass(int pass) {
        Grid grid = World.Instance.Grid;

        for (int layer = 0; layer < grid.Size[0]+grid.Size[1]-1; layer++) {
            for (int x = 0; x < layer+1; x++) {
                int y = layer-x;
                ExecuteGridCellRenderCommand(x, y, true, pass);
            }
        }
    }

    private void ExecuteGridCellRenderCommand(int x, int y, bool recursive, int pass) {
        if (!_cellRenderCommands.ContainsKey((x,y))) return;

        CellRenderCommands cell = _cellRenderCommands[(x,y)];
        if (pass == 0 && cell.RenderedContents) return;

        if (pass != 0) {
            cell.RenderContents(_spriteBatch, pass);
            return;
        }

        if (_cellRenderCommands.TryGetValue((x-1,y), out CellRenderCommands nCell)) {
            if (!nCell.PreparingToRenderContents) ExecuteGridCellRenderCommand(x-1, y, true, pass);
        }
        if (_cellRenderCommands.TryGetValue((x,y-1), out CellRenderCommands n2Cell)) {
            if (!n2Cell.PreparingToRenderContents) ExecuteGridCellRenderCommand(x, y-1, true, pass);
        }

        cell.RenderTile(_spriteBatch);

        if (!recursive) return;


        cell.PrepareToRenderContents();
        if (_cellRenderCommands.TryGetValue((x,y+1), out CellRenderCommands dCell)) {
            if (!dCell.RenderedContents && dCell.Lift <= cell.Lift) ExecuteGridCellRenderCommand(x, y+1, false, pass);
        }
        if (_cellRenderCommands.TryGetValue((x+1,y), out CellRenderCommands d2Cell)) {
            if (!d2Cell.RenderedContents && d2Cell.Lift <= cell.Lift) ExecuteGridCellRenderCommand(x+1, y, false, pass);
        }
        
        cell.RenderContents(_spriteBatch, pass);
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