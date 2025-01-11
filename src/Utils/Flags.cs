namespace QMEditor;

public static class Flags {

    private static bool[] _flags = [false];

    public static void Set(Flag flag) {
        _flags[(int)flag] = true;
    }

    public static bool Read(Flag flag) {
        bool r = _flags[(int)flag];
        _flags[(int)flag] = false;
        return r;
    }

}

public enum Flag {
    SaveAsPng = 0
}