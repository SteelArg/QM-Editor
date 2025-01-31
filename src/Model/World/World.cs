namespace QMEditor.Model;

public class World : Singleton<World> {

    public readonly WorldSettings Settings;
    public static SharedWorldCursor Cursor { get => Instance._sharedCursor; }

    private Grid _grid;
    private SharedWorldCursor _sharedCursor;

    public Grid Grid {get => _grid;}

    public World(WorldSettings settings) : base() {
        Settings = settings;
        _grid = new Grid(settings.Size);
        _sharedCursor = new SharedWorldCursor();
    }

}