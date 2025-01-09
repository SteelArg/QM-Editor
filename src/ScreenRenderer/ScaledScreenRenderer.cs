using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor;

[Obsolete("Scaled screen renderer is deprecated. It does not work with Myra UI")]
public class ScaledScreenRenderer : ScreenRenderer {

    private RenderTarget2D _screenRenderTarget;

    private int[] _renderSize;

    public ScaledScreenRenderer(Game game, int[] windowSize, int[] renderSize) : base(game, windowSize) {
        _renderSize = renderSize;
    }

    public override void Load() {
        base.Load();
        _screenRenderTarget = new RenderTarget2D(_game.GraphicsDevice, _renderSize[0], _renderSize[1]);
    }

    public override void Draw(GameTime gameTime) {
        _game.GraphicsDevice.SetRenderTarget(_screenRenderTarget);
        base.Draw(gameTime);

        // Render to the window itself
        _game.GraphicsDevice.SetRenderTarget(null);
        _game.GraphicsDevice.Clear(Color.Green);

        float calculatedScale = _windowSize[1] / (float)_renderSize[1];

        _spriteBatch.Begin();
        _spriteBatch.Draw(_screenRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, calculatedScale, SpriteEffects.None, 0f);
        _spriteBatch.End();
    }

}
