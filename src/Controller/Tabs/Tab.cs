using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;

namespace QMEditor.Controllers;

public abstract class Tab {

    protected Widget _widget;
    protected TabsManager _manager;

    public Widget Widget { get => _widget; }

    public Tab() {}

    public void SetManager(TabsManager manager) => _manager = manager;

    public virtual void Load() {
        _widget = BuildUI();
        if (_widget == null) _widget = new Widget();
    }

    protected abstract Widget BuildUI();

    public abstract void Open();
    public abstract void Close();

    public abstract void Draw(SpriteBatch spritebatch);

}