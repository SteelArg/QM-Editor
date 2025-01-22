using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;

namespace QMEditor;

public static class Global {

    private static Game _game;
    private static Desktop _desktop;

    public static Game Game { get => _game; }
    public static Desktop Desktop { get => _desktop; }

    public static void SetGame(Game game) {
        MyraEnvironment.Game = game;
        _game = game;
    }

    public static void SetDesktop(Desktop desktop) {
        _desktop = desktop;
    }

}