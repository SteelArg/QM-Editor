using Microsoft.Xna.Framework.Input;

namespace QMEditor.View;

public class KeyboardInput {

    private KeyboardState _oldState;

    public KeyboardInput() {}

    public void LoadNextState() {
        _oldState = Keyboard.GetState();
    }

    public bool KeyHeld(Keys key) {
        return Keyboard.GetState().IsKeyDown(key);
    }

}