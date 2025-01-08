using Microsoft.Xna.Framework;

namespace QMEditor;

public class World : Singleton<World> {

    private Grid _grid;

    private Vector2 _gridSize;

    public World(int sizeX, int sizeY) : base() {
        _gridSize = new Vector2(sizeX, sizeY);
    }

    protected override void OnSingletonCreated() {
        _grid = new Grid(_gridSize);
    }

    public void Hello() {}

}