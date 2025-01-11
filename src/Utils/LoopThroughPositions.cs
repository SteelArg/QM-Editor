using System;
using Microsoft.Xna.Framework;

public static class LoopThroughPositions {

    public static void Every(Action<int, int> action, Vector2 loopSize) {
        Every(action, [(int)loopSize.X, (int)loopSize.Y]);
    }

    public static void Every(Action<int, int> action, int[] loopSize) {
        for (int x = 0; x < loopSize[0]; x++) {
            for (int y = 0; y < loopSize[1]; y++) {
                action(x, y);
            }
        }
    }

}