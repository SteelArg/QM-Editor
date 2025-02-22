using Microsoft.Xna.Framework;
using QMEditor;

public static class AppLayout {

    public static int TabSelectHeight = 100;
    public static int InspectorWidth = 500;

    public static Vector2 DrawPos => new Vector2(0, TabSelectHeight);
    public static Vector2 DrawSize => new Vector2(Resolution.Current[0] - InspectorWidth, Resolution.Current[1]-TabSelectHeight);

}