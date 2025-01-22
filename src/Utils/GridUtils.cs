using QMEditor.Model;

namespace QMEditor;

public static class GridPositionExtensions {

    public static int[] ValidateGridPosition(this int[] originalPos, int[] worldSize = null) {
        worldSize = worldSize ?? World.Instance.Grid.Size;
        if (originalPos[0] < 0 || originalPos[0] >= worldSize[0]) return null;
        if (originalPos[1] < 0 || originalPos[1] >= worldSize[1]) return null;
        return originalPos;
    }

}