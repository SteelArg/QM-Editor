using QMEditor.Model;

namespace QMEditor.Controllers;

public static class WorldSaver {

    private const string size = "size";
    private const string gridObject = "grid_objects_";

    public static void Save() {
        var parser = new StringDataParser("saves\\test.qmworld");
        
        Grid grid = World.Instance.Grid;
        parser.SetValue(size, $"{grid.Size[0]};{grid.Size[1]}");
        
        foreach (GridCell cell in grid.GetGridCells()) {
            string cellLocation = $"{cell.Position[0]}_{cell.Position[1]}";
            string objects = "";

            foreach (GridObject obj in cell.Objects) {
                string type = obj.GetType().Name;
                string asset = "none";
                string addData = "none";

                RenderableGridObject renderableObj = obj as RenderableGridObject;
                if (renderableObj != null) {
                    asset = renderableObj.Asset.NameOfFile;
                    if (renderableObj is Character) {
                        addData = "";
                        Character character = renderableObj as Character;
                        foreach (Accessory accessory in character.Accessories){
                            addData += $"{accessory.Asset.NameOfFile},";
                        }
                        if (addData.Length > 0)
                            addData = addData.Remove(addData.Length-1);
                    }
                }
                
                string objectData = $"{type};{asset}:{addData}";
                objects += objectData + "|";
            }
            objects = objects.Remove(objects.Length-1);
            parser.SetValue(gridObject + cellLocation, objects);
        }

        parser.Save();
    }

}