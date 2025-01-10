namespace QMEditor.Model;

public struct WorldSettings {

    public const int DefaultSize = 8;
    public static readonly WorldSettings Default = new WorldSettings([DefaultSize, DefaultSize]);

    public int[] WorldSize;

    public WorldSettings(int[] worldSize) {
        WorldSize = worldSize;
    }

}