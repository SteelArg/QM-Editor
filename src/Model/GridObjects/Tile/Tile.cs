using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Tile : RenderableGridObject {

    [AddToInspection(InspectionProperty.PropertyType.Integer)]
    public int LiftOffset { get; set; } = 0;

    public Tile(AssetBase asset) : base(asset) {}
    public Tile(Dictionary<string, string> stringData) {
        LoadFromString(stringData);
    }

    public override GridObject Clone() {
        var tile = new Tile(_asset);
        tile.LiftOffset = LiftOffset;
        return tile;
    }

    public int GetLift(GridRenderSettings renderSettings) {
        return renderSettings.CalculateTileLift(_asset.GetSize()[1]) + LiftOffset;
    }

    protected override Vector2 GetRenderPos(GridObjectRenderData renderData) {
        return renderData.RenderSettings.CalculateTilePosition(GridPosition, _asset.GetSize());
    }

    public override Dictionary<string, string> SaveToString(Dictionary<string, string> existingData = null) {
        existingData = base.SaveToString(existingData);
        existingData.Add("LiftOffset", LiftOffset.ToString());
        return existingData;
    }

    protected override void LoadFromString(Dictionary<string, string> stringData) {
        base.LoadFromString(stringData);
        LiftOffset = int.Parse(stringData.GetValueOrDefault("LiftOffset") ?? "0");
    }

}

public class TileFactory {

    private Tile _tile;

    public TileFactory(Tile tile) {
        _tile = tile;
    }

    public TileFactory(AssetBase asset) {
        _tile = new Tile(asset);
    }

    public Tile Create() => (Tile)_tile.Clone();

    public void FillGrid(Grid grid) {
        LoopThroughPositions.Every((x,y) => {
            grid.PlaceOnGrid(_tile.Clone(), [x,y]);
        }, grid.Size);
    }

}