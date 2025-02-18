using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor.Controllers;

public class DefaultEditKeybindsGenerator : IEditKeybindsGenerator {
    
    private readonly WorldEditor _worldEditor;

    public DefaultEditKeybindsGenerator(WorldEditor worldEditor) {
        _worldEditor = worldEditor;
    }

    public List<EditKeybind> Generate() {
        List<EditKeybind> keybinds = [
            new EditKeybind(()=>Input.MouseButtonClicked(0), ()=>new PlaceGridObjectCommand(_worldEditor.GetEditContext(), World.Cursor.GetCopyOfObject())),
            new EditKeybind(()=>Input.MouseButtonClicked(1), ()=>new ClearGridCellCommand(_worldEditor.GetEditContext(), Input.KeyHeld(Keys.LeftShift))),
            new EditKeybind(()=>Input.MouseButtonClicked(2), ()=>new CopyGridObjectCommand(_worldEditor.GetEditContext(), Input.KeyHeld(Keys.LeftShift))),
            .. GetGridPositionOperationKeybinds(),
        ];
        return keybinds;
    }

    private EditKeybind[] GetGridPositionOperationKeybinds() {
        return [
            new EditKeybind(()=>Input.KeyFired(Keys.E), ()=>new RotateGridCommand(_worldEditor.GetEditContext(), true)),
            new EditKeybind(()=>Input.KeyFired(Keys.Q), ()=>new RotateGridCommand(_worldEditor.GetEditContext(), false)),
            new EditKeybind(()=>Input.KeyFired(Keys.Left), ()=>new MoveGridCommand(_worldEditor.GetEditContext(), [-1, 0])),
            new EditKeybind(()=>Input.KeyFired(Keys.Up), ()=>new MoveGridCommand(_worldEditor.GetEditContext(), [0, -1])),
            new EditKeybind(()=>Input.KeyFired(Keys.Right), ()=>new MoveGridCommand(_worldEditor.GetEditContext(), [1, 0])),
            new EditKeybind(()=>Input.KeyFired(Keys.Down), ()=>new MoveGridCommand(_worldEditor.GetEditContext(), [0, 1]))
        ];
    }

}