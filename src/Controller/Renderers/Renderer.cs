using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Controllers;

public abstract class Renderer {

    public abstract RenderTarget2D Render(SpriteBatch spriteBatch);

}

public class RenderList {

    private readonly List<Renderer> _renderers;
    private readonly SpriteBatch _spriteBatch;

    public RenderList() {
        _renderers = new List<Renderer>();
        _spriteBatch = new SpriteBatch(Global.Game.GraphicsDevice);
    }

    public void AddRenderer(Renderer renderer) => _renderers.Add(renderer);
    public void RemoveRenderer(Renderer renderer) => _renderers.Remove(renderer);

    public void Render(SpriteBatch spriteBatch = null) {
        foreach (Renderer renderer in _renderers) {
            renderer.Render(spriteBatch);
        }
    }

    public RenderTarget2D RenderToTarget(int[] targetSize) {
        RenderTarget2D target = new RenderTarget2D(Global.Game.GraphicsDevice, targetSize[0], targetSize[1]);
        
        RenderTarget2D[] renderTargets = new RenderTarget2D[_renderers.Count];
        
        for (int i = 0; i < _renderers.Count; i++) {
            renderTargets[i] = _renderers[i].Render(_spriteBatch);
        }

        Global.Game.GraphicsDevice.SetRenderTarget(target);
        Global.Game.GraphicsDevice.Clear(Color.Transparent);

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

        for (int i = 0; i < _renderers.Count; i++) {
            if (renderTargets[i] == null) continue;
            _spriteBatch.Draw(renderTargets[i], Vector2.Zero, Color.White);
        }

        _spriteBatch.End();

        for (int i = 0; i < _renderers.Count; i++) {
            renderTargets[i]?.Dispose();
        }

        return target;
    }

}