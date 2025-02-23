using System.Collections.Generic;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class WorldSaver {

    public void Save(string path = "saves\\default.qmworld") {
        var parser = new StringDataParser(path);
        parser.SetValue("editor_version", AppSettings.Version.Get());

        // Shader
        if (WorldEffectManager.CurrentEffect != null) {
            parser.SetValue("shader_path", $"\"{WorldEffectManager.CurrentEffect.GetShaderPath()}\"");
            parser.SetValue("shader_user_variable", WorldEffectManager.CurrentEffect.GetUserVariable().ToString());
        }

        // Grid
        Grid grid = World.Instance.Grid;
        parser.SetValue("size", $"{grid.Size[0]};{grid.Size[1]}");
        
        foreach (GridCell cell in grid.GetGridCells()) {
            string cellLocation = $"{cell.Position[0]}_{cell.Position[1]}";
            string objects = "";

            foreach (GridObject obj in cell.Objects) {
                Dictionary<string, string> objData = obj.SaveToString();

                string stringData = "";
                foreach (string key in objData.Keys) {
                    stringData += $"{key}:{objData[key]};";
                }
                if (stringData.Length > 0)
                    stringData = stringData.Remove(stringData.Length-1);
                
                objects += stringData + "|";
            }
            if (objects.Length > 0)
                objects = objects.Remove(objects.Length-1);
            parser.SetValue("grid_objects_" + cellLocation, objects);
        }

        parser.Save();
    }

}