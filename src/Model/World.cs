namespace QMEditor.Model;

public class World : Singleton<World> {

    public WorldSettings Settings;

    private Grid _grid;

    public Grid Grid {get => _grid;}

    public World(WorldSettings settings) : base() {
        Settings = settings;
        _grid = new Grid(settings.Size);
    }

}