using System;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class EditKeybind {

    private Func<EditContext, bool> _keybind;
    private Func<EditContext, IEditCommand> _commandFactory;

    public EditKeybind(Func<EditContext, bool> keybind, Func<EditContext, IEditCommand> commandFactory) {
        _keybind = keybind;
        _commandFactory = commandFactory;
    }

    public IEditCommand CreateCommandIfKeybindFired(EditContext ctx) {
        if (!_keybind.Invoke(ctx)) return null;
        return _commandFactory.Invoke(ctx);
    }

}