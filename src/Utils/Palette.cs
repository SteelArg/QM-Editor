using Microsoft.Xna.Framework;

public static class Palette {

    public static readonly float[] HoverColor = [0.16f, 0.27f, 0.57f, 1f];
    public static readonly float[] PlacingColor = [30f, 3f, 3f, 0.4f];

    public static Color ToColor(float[] floatColor) {
        return new Color(floatColor[0], floatColor[1], floatColor[2], floatColor[3]);
    }

}