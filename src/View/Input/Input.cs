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
        return Instance._mouse.IsMouseButtonClicked(mouseButton);
    }

    public static bool MouseButtonHeld(int mouseButton = 0) {
        return Instance._mouse.IsMouseButtonHeld(mouseButton);
    }

    public static bool KeyHeld(Keys key) {
        return Instance._keyboard.IsKeyHeld(key);
    }

    public static bool KeyFired(Keys key) {
        return Instance._keyboard.IsKeyFired(key);
    }

}