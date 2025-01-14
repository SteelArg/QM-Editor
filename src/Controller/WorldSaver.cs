using QMEditor.Model;

namespace QMEditor.Controllers;

public static class WorldSaver {

    private const string size = "size";
    private const string gridObject = "grid_objects_";

    public static void Save() {
        var parser = new StringDataParser("saves\\test.qmworld");
        
        Grid grid = World.Instance.Grid;
        parser.SetValue(size, $"{grid.Size.X};{grid.Size.Y}");
        
        foreach (GridCell cell in grid.GetGridCells()) {
            string cellLocation = $"{cell.Position.X}_{cell.Position.Y}";
            string objects = "";

            foreach (GridObject obj in cell.Objects) {
                string type = obj.GetType().Name;
                string asset = "none";
                RenderableGridObject renderableObj = obj as RenderableGridObject;
                if (renderableObj != null)
                    asset = renderableObj.Asset.NameOfFile;
                
                string objectData = $"{type};{asset}";
                objects += objectData + "|";
            }
            objects = objects.Remove(objects.Length-1);
            parser.SetValue(gridObject + cellLocation, objects);
        }

        parser.Save();
    }

}