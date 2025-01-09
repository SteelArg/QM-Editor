using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor;

public abstract class Renderer {

    public abstract void Render(SpriteBatch spriteBatch);

}

public class RenderList {

    private List<Renderer> _renderers;

    public RenderList() {
        _renderers = new List<Renderer>();
    }

    public void AddRenderer(Renderer renderer) => _renderers.Add(renderer);
    public void RemoveRenderer(Renderer renderer) => _renderers.Remove(renderer);

    public void Render(SpriteBatch spriteBatch = null) {
        foreach (Renderer renderer in _renderers) {
            renderer.Render(spriteBatch);
        }
    }

}