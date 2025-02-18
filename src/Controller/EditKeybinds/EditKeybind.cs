using System;
using QMEditor.Model;

namespace QMEditor.Controllers;

public class EditKeybind {

    private Func<bool> _keybind;
    private Func<IEditCommand> _commandFactory;

    public EditKeybind(Func<bool> keybind, Func<IEditCommand> commandFactory) {
        _keybind = keybind;
        _commandFactory = commandFactory;
    }

    public IEditCommand CreateCommandIfKeybindFired() {
        if (!_keybind.Invoke()) return null;
        return _commandFactory.Invoke();
    }

}