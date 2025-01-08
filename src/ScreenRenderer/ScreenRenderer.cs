using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor;

public class ScreenRenderer {

    public Action<SpriteBatch> OnRender;

    private GraphicsDeviceManager _graphics;
    protected SpriteBatch _spriteBatch;
    protected Game _game;

    protected int[] _windowSize;

    public ScreenRenderer(Game game, int[] windowSize) {
        _game = game;
        _graphics = new GraphicsDeviceManager(game);
        _windowSize = windowSize;
    }

    public void Init() {
        _graphics.PreferredBackBufferWidth = _windowSize[0];
        _graphics.PreferredBackBufferHeight = _windowSize[1];
        _graphics.ApplyChanges();
    }

    public virtual void Load() {
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
    }

    public virtual void Draw(GameTime gameTime) {
        _game.GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
        OnRender?.Invoke(_spriteBatch);
        _spriteBatch.End();
    }

}