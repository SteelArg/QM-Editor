using QMEditor.Model;

namespace QMEditor;

public static class GridPositionExtensions {

    public static int[] ValidateGridPosition(this int[] originalPos, int[] worldSize = null) {
        worldSize = worldSize ?? World.Instance.Grid.Size;
        if (originalPos[0] < 0 || originalPos[0] >= worldSize[0]) return null;
        if (originalPos[1] < 0 || originalPos[1] >= worldSize[1]) return null;
        return originalPos;
    }

    public static bool IsSameAs(this int[] pos1, int[] pos2) {
        if (pos1 == null && pos2 == null) return true;
        if (pos1 == null || pos2 == null) return false;

        return pos1[0] == pos2[0] && pos1[1] == pos2[1];
    }

}