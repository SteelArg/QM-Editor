using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldLoader : Singleton<WorldLoader> {

    public void Load(string path = "saves\\default.qmworld") {
        var parser = new StringDataParser(path);
        parser.Load();

        string[] size = parser.GetValue("size").Split(';');
        var worldSettings = new WorldSettings([int.Parse(size[0]), int.Parse(size[1])]);
        new World(worldSettings);

        LoopThroughPositions.Every((x, y) => {
            string cellName = $"grid_objects_{x}_{y}";
            foreach (string gridObjectString in parser.GetValue(cellName).Split("|")) {
                if (gridObjectString == string.Empty) continue;
                string typeString = gridObjectString.Split(';')[0];
                string assetString = gridObjectString.Split(';')[1].Split(':')[0];
                string[] addDataStrings = gridObjectString.Split(';')[1].Split(':')[1].Split(',');

                Asset asset = AssetsLoader.Instance.GetAsset(assetString, AssetsFolders.All);
                GridObject gridObject = null;
                if (typeString == typeof(Character).Name) {
                    Character character = new Character(asset);
                    foreach (string dataString in addDataStrings) {
                        if (dataString == string.Empty) continue;
                        Asset accessoryAsset = AssetsLoader.Instance.GetAsset(dataString, AssetsFolders.Accessories); 
                        Accessory accessory = new Accessory(accessoryAsset);
                        character.AddAccessory(accessory);
                    }
                    gridObject = character;
                }
                else if (typeString == typeof(Tile).Name) {
                    gridObject = new Tile(asset);
                }
                else if (typeString == typeof(Prop).Name) {
                    gridObject = new Prop(asset);
                }

                World.Instance.Grid.PlaceOnGrid(gridObject, [x,y]);
            }
        }, worldSettings.Size);
    }
 
}