using Microsoft.Xna.Framework.Input;

namespace QMEditor.View;

public class Input : Singleton<Input> {
    
    private MouseInput _mouse;
    private KeyboardInput _keyboard;

    public Input() {
        _mouse = new MouseInput();
        _keyboard = new KeyboardInput();
    }

    public static void LateUpdate() {
        Instance._mouse.LoadNextState();
        Instance._keyboard.LoadNextState();
    }

    public static bool MouseButtonClicked(int mouseButton = 0) {
        return Instance._mouse.MouseButtonClicked(mouseButton);
    }

    public static bool MouseButtonHeld(int mouseButton = 0) {
        return Instance._mouse.MouseButtonHeld(mouseButton);
    }

    public static bool KeyHeld(Keys key) {
        return Instance._keyboard.KeyHeld(key);
    }

}