using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QMEditor.Controllers;

namespace QMEditor;

public class ScreenRenderer {

    public RenderList UIRenderList = new RenderList();
    public RenderList SpriteRenderList = new RenderList();

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
        DrawUI();
        DrawSprites();
    }

    protected void DrawSprites() {
        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
        SpriteRenderList.Render(_spriteBatch);
        _spriteBatch.End();
    }

    protected void DrawUI() {
        UIRenderList.Render();
    }

}