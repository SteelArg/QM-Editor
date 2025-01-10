namespace QMEditor;

public static class Resolution {

    public static readonly int[] HD = [1920, 1080];
    public static readonly int[] TwoK = [2560, 1440];
    public static readonly int[] Small = [1280, 720];

    private static int[] _current;
    public static int[] Current {get => _current;}

    public static int[] Pick(int[] resolution) {
        _current = resolution;
        return resolution;
    }

}