using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class World : Singleton<World> {

    public WorldSettings Settings;

    private Grid _grid;
    private Vector2 _gridSize;

    public Grid Grid {get => _grid;}

    public World(WorldSettings settings) : base() {
        Settings = settings;
        _gridSize = new Vector2(settings.WorldSize[0], settings.WorldSize[1]);
        _grid = new Grid(_gridSize);
    }

}