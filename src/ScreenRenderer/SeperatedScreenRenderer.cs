using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor;

public class SeperatedScreenRenderer : ScreenRenderer {

    private RenderTarget2D _spritesRenderTarget;

    public SeperatedScreenRenderer(Game game, int[] windowSize) : base(game, windowSize) {}

    public override void Load() {
        base.Load();
        _spritesRenderTarget = new RenderTarget2D(_game.GraphicsDevice, _windowSize[0], _windowSize[1]);
    }

    public override void Draw(GameTime gameTime) {
        // Sprites to seperate render target
        _game.GraphicsDevice.SetRenderTarget(_spritesRenderTarget);
        _game.GraphicsDevice.Clear(Color.Black);
        DrawSprites();
        _game.GraphicsDevice.SetRenderTarget(null);
        
        // UI
        _game.GraphicsDevice.Clear(Color.Black);
        DrawUI();

        // All sprites onto the screen
        _spriteBatch.Begin();
        _spriteBatch.Draw(_spritesRenderTarget, AppLayout.DrawPos, AppLayout.DrawSize, Color.White);
        _spriteBatch.End();
    }

}