using System;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldLoader {

    public void Load(string path = "saves\\default.qmworld") {
        var parser = new StringDataParser(path);
        parser.Load();
        string saveVersion = parser.GetValue("editor_version");
        if (saveVersion != AppSettings.Version.Value) {
            Console.WriteLine("Can not open saves from unmatching editor versions");
            return;
        }

        string[] size = parser.GetValue("size").Split(';');
        var worldSettings = new WorldSettings([int.Parse(size[0]), int.Parse(size[1])]);
        new World(worldSettings);

        LoopThroughPositions.Every((x, y) => {
            string cellName = $"grid_objects_{x}_{y}";
            foreach (string gridObjectString in parser.GetValue(cellName).Split("|")) {
                if (gridObjectString == string.Empty) continue;
                string typeString = gridObjectString.Split(';')[0];
                string assetString = gridObjectString.Split(';')[1];
                string[] addDataStrings = gridObjectString.Split(';')[2].Split(':');

                Asset asset = AssetsLoader.Instance.GetAsset(assetString, AssetsFolders.All);
                GridObject gridObject = null;
                if (typeString == typeof(Character).Name) {
                    Character character = new Character(asset);
                    foreach (string dataString in addDataStrings) {
                        if (dataString == string.Empty) continue;
                        string accessoryAssetName = dataString.Split(',')[0];
                        string accessoryLift = dataString.Split(',')[1];
                        Asset accessoryAsset = AssetsLoader.Instance.GetAsset(accessoryAssetName, AssetsFolders.Accessories); 
                        Accessory accessory = new Accessory(accessoryAsset, int.Parse(accessoryLift));
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