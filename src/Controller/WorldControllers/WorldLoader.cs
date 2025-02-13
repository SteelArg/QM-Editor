using System;
using System.Collections.Generic;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldLoader {

    public void Load(string path = "saves\\default.qmworld") {
        var parser = new StringDataParser(path);
        parser.Load();
        try {
        string saveVersion = parser.GetValue("editor_version");
        if (saveVersion != AppSettings.Version.Get())
            throw new Exception();
        } catch {
            ServiceLocator.MessageWindowsService.ErrorWindow($"Can not open save from different QM Editor versions.\nCurrent version: {AppSettings.Version.Get()}.");
            return;
        }

        string[] size = parser.GetValue("size").Split(';');
        var worldSettings = new WorldSettings([int.Parse(size[0]), int.Parse(size[1])]);
        new World(worldSettings);

        LoopThroughPositions.Every((x, y) => {
            string cellName = $"grid_objects_{x}_{y}";
            foreach (string gridObjectString in parser.GetValue(cellName).Split("|")) {
                if (gridObjectString == string.Empty) continue;

                Dictionary<string, string> objectData = new();

                foreach (string keyValuePair in gridObjectString.Split(';')) {
                    if (keyValuePair == string.Empty) continue;
                    string key = keyValuePair.Split(':')[0];
                    string value = keyValuePair.Split(':')[1];
                    objectData[key] = value;
                }

                string typeName = objectData["Type"];

                GridObject gridObject = null;

                switch (typeName) {
                    case "Character":
                        gridObject = new Character(objectData);
                        break;
                    case "Tile":
                        gridObject = new Tile(objectData);
                        break;
                    case "Prop":
                        gridObject = new Prop(objectData);
                        break;
                }

                World.Instance.Grid.PlaceOnGrid(gridObject, [x,y]);                
            }
        }, worldSettings.Size);
    }
 
}