using Microsoft.Xna.Framework;
using Myra;

namespace QMEditor;

public static class Global {

    private static Game _game;

    public static Game Game { get => _game; }

    public static void SetGame(Game game) {
        MyraEnvironment.Game = game;
        _game = game;
    }

}