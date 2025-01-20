using Microsoft.Xna.Framework.Input;

namespace QMEditor.View;

public class MouseInput {

    private MouseState _oldState;

    public MouseInput() {}

    public void LoadNextState() {
        _oldState = Mouse.GetState();
    }

    public bool MouseButtonClicked(int mouseButton) {
        MouseState mouseState = Mouse.GetState();
        switch (mouseButton) {
            case 0:
                return _oldState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed;
            case 1:
                return _oldState.RightButton == ButtonState.Released && mouseState.RightButton == ButtonState.Pressed;
            default:
                return false;
        }
    }

    public bool MouseButtonHeld(int mouseButton) {
        MouseState mouseState = Mouse.GetState();
        switch (mouseButton) {
            case 0:
                return mouseState.LeftButton == ButtonState.Pressed;
            case 1:
                return mouseState.RightButton == ButtonState.Pressed;
            default:
                return false;
        }
    }

}