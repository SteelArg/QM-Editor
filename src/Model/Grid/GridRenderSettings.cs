using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public struct GridRenderSettings {

    public static readonly GridRenderSettings Default = new GridRenderSettings(new Vector2(100f, 50f), new Vector2(16f, 9f));

    public readonly Vector2 Offset;
    public readonly Vector2 TileTopSize;

    public readonly Vector2 StepX { get; }
    public readonly Vector2 StepY { get; }

    private static readonly Dictionary<Type, float> _depth = new Dictionary<Type, float>() { {typeof(Character), 0.5f}, {typeof(Tile), 0f}, {typeof(Accessory), 0.7f} };
    private const int spriteLift = -3;

    public GridRenderSettings(Vector2 offset, Vector2 tileTopSize) {
        Offset = offset;
        TileTopSize = tileTopSize;
        StepX = Vector2.Floor(TileTopSize/2f);
        StepY = new Vector2(-StepX.X, StepX.Y);
    }

    public Vector2 CalculateRenderPosition(int[] gridCell, int[] spriteSize) {
        Vector2 pos = CalculateTilePosition(gridCell);
        pos += StepX;

        pos += new Vector2(MathF.Ceiling(-spriteSize[0]/2f), -spriteLift-spriteSize[1]);

        return pos;
    }

    public Vector2 CalculateTilePosition(int[] gridCell) {
        Vector2 pos = Vector2.Zero;

        pos += gridCell[0] * StepX;
        pos += gridCell[1] * StepY;

        return pos + Offset;
    }

    public int[] ScreenPositionToGrid(Vector2 screen) {
        Vector2 localPos = screen - Offset;
        
        float x1 = localPos.X - StepX.X;
        float y1 = localPos.Y * StepY.X/StepY.Y;
        // Apply a counter-clockwise rotation of 45 degrees
        float xr = MathF.Cos(MathF.PI/4)*x1 - MathF.Sin(MathF.PI/4)*y1;
        float yr = MathF.Sin(MathF.PI/4)*x1 + MathF.Cos(MathF.PI/4)*y1;

        float diag = StepX.X * MathF.Sqrt(2f);
        int x2 = (int)(xr / diag);
        int y2 = (int)(yr * -1 / diag);
        
        return [x2, y2];
    }

    public float GetDepthFor<T>() {
        return GetDepthFor(typeof(T));
    }

    public float GetDepthFor(Type type) {
        float depth;
        _depth.TryGetValue(type, out depth);
        return depth;
    }

}