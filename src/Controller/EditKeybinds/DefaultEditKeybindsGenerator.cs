using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor.Controllers;

public class DefaultEditKeybindsGenerator : IEditKeybindsGenerator {

    public List<EditKeybind> Generate() {
        List<EditKeybind> keybinds = [
            new EditKeybind((ctx)=>KeybindHelper.MouseClickOverTile(0, ctx), (ctx)=>new PlaceGridObjectCommand(ctx, World.Cursor.GetCopyOfObject())),
            new EditKeybind((ctx)=>KeybindHelper.MouseClickOverTile(1, ctx), (ctx)=>new ClearGridCellCommand(ctx, Input.KeyHeld(Keys.LeftShift))),
            new EditKeybind((ctx)=>KeybindHelper.MouseClickOverTile(2, ctx), (ctx)=>new CopyGridObjectFromCellCommand(ctx, Input.KeyHeld(Keys.LeftShift))),

            new EditKeybind((ctx)=>KeybindHelper.KeyPressWhileHolding(Keys.C, Keys.LeftAlt), (ctx)=>new ClearCursorCommand(ctx)),

            .. GetGridPositionOperationKeybinds(),
        ];
        return keybinds;
    }

    private EditKeybind[] GetGridPositionOperationKeybinds() {
        return [
            new EditKeybind((ctx)=>KeybindHelper.KeyPressWhileHolding(Keys.E, Keys.LeftAlt), (ctx)=>new RotateGridCommand(ctx, true)),
            new EditKeybind((ctx)=>KeybindHelper.KeyPressWhileHolding(Keys.Q, Keys.LeftAlt), (ctx)=>new RotateGridCommand(ctx, false)),
            new EditKeybind((ctx)=>KeybindHelper.KeyPressWhileHolding(Keys.A, Keys.LeftAlt), (ctx)=>new MoveGridCommand(ctx, [-1, 0])),
            new EditKeybind((ctx)=>KeybindHelper.KeyPressWhileHolding(Keys.W, Keys.LeftAlt), (ctx)=>new MoveGridCommand(ctx, [0, -1])),
            new EditKeybind((ctx)=>KeybindHelper.KeyPressWhileHolding(Keys.D, Keys.LeftAlt), (ctx)=>new MoveGridCommand(ctx, [1, 0])),
            new EditKeybind((ctx)=>KeybindHelper.KeyPressWhileHolding(Keys.S, Keys.LeftAlt), (ctx)=>new MoveGridCommand(ctx, [0, 1]))
        ];
    }

}