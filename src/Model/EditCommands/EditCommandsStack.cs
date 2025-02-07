using System.Collections.Generic;

namespace QMEditor.Model;

public class EditCommandsStack {

    private Stack<IEditCommand> commandsStack;

    public EditCommandsStack() {
        commandsStack = new Stack<IEditCommand>();
    }

    public void AddAndExecuteCommand(IEditCommand command) {
        if (command == null) return;
        
        command.Do();
        if (!command.IsEmpty())
            commandsStack.Push(command);
    }

    public void UndoLastCommand() {
        if (commandsStack.Count > 0)
            commandsStack.Pop().Undo();
    }
    
}
