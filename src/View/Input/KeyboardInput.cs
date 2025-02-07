using Microsoft.Xna.Framework.Input;

namespace QMEditor.View;

public class KeyboardInput {

    private KeyboardState _oldState;

    public KeyboardInput() {}

    public void LoadNextState() {
        _oldState = Keyboard.GetState();
    }

    public bool IsKeyHeld(Keys key) {
        return Keyboard.GetState().IsKeyDown(key);
    }

    public bool IsKeyFired(Keys key) {
        return Keyboard.GetState().IsKeyUp(key) && _oldState.IsKeyDown(key);
    }

}