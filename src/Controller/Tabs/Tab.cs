using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Controllers;

public abstract class Tab {

    public Tab() {

    }

    public abstract void Open();
    public abstract void Close();

    public abstract void Draw(SpriteBatch spritebatch);

}