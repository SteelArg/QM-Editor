using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldSaver {

    public void Save(string path = "saves\\default.qmworld") {
        var parser = new StringDataParser(path);
        parser.SetValue("editor_version", AppSettings.Version.Value);
        
        Grid grid = World.Instance.Grid;
        parser.SetValue("size", $"{grid.Size[0]};{grid.Size[1]}");
        
        foreach (GridCell cell in grid.GetGridCells()) {
            string cellLocation = $"{cell.Position[0]}_{cell.Position[1]}";
            string objects = "";

            foreach (GridObject obj in cell.Objects) {
                string type = obj.GetType().Name;
                string asset = "none";
                string addData = "none";

                RenderableGridObject renderableObj = obj as RenderableGridObject;
                if (renderableObj != null) {
                    asset = renderableObj.Asset.Name;
                    if (renderableObj is Character character) {
                        addData = $"CHARACTER_VARIATION,{character.Variation}:";
                        foreach (Accessory accessory in character.Accessories){
                            addData += $"{accessory.Asset.Name},{accessory.Lift}:";
                        }
                        if (addData.Length > 0)
                            addData = addData.Remove(addData.Length-1);
                    }
                }
                
                string objectData = $"{type};{asset};{addData}";
                objects += objectData + "|";
            }
            if (objects.Length > 0)
                objects = objects.Remove(objects.Length-1);
            parser.SetValue("grid_objects_" + cellLocation, objects);
        }

        parser.Save();
    }

}