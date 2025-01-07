using Microsoft.Xna.Framework.Graphics;

public abstract class Tab {

    public Tab() {

    }

    public abstract void Open();
    public abstract void Close();

    public abstract void Draw(SpriteBatch spritebatch);

}