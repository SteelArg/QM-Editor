using Microsoft.Xna.Framework;
using QMEditor;

public static class AppLayout {

    public static int TabSelectHeight = 100;
    public static int InspectorWidth = 500;

    public static Vector2 DrawPos => new Vector2(0, TabSelectHeight);
    public static Rectangle DrawSize => new Rectangle(0, 0, Resolution.HD[0] - InspectorWidth, Resolution.HD[1]-TabSelectHeight);

}