using Microsoft.Xna.Framework.Input;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor;

public static class KeybindHelper {

    public static bool MouseClickOverTile(int mouseButton, EditContext ctx) => Input.MouseButtonClicked(mouseButton) || Input.MouseButtonHeld(mouseButton) && ctx.HoveredThisFrame;

    public static bool KeyPressOverTile(Keys keys, EditContext ctx) => Input.KeyFired(keys) || Input.KeyHeld(keys) && ctx.HoveredThisFrame;

    public static bool KeyPressWhileHolding(Keys keys, Keys holdingKey) => Input.KeyFired(keys) && Input.KeyHeld(holdingKey);

}