using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor;

public class SeperatedScreenRenderer : ScreenRenderer {

    private RenderTarget2D _spritesRenderTarget;

    private float _scale;

    public SeperatedScreenRenderer(Game game, int[] windowSize, float scale = 1f) : base(game, windowSize) {
        _scale = scale;
    }

    public override void Load() {
        base.Load();
        _spritesRenderTarget = new RenderTarget2D(_game.GraphicsDevice, _windowSize[0], _windowSize[1]);
    }

    public override void Draw(GameTime gameTime) {
        // Sprites to seperate render target
        _game.GraphicsDevice.SetRenderTarget(_spritesRenderTarget);
        _game.GraphicsDevice.Clear(Color.Transparent);
        DrawSprites();
        _game.GraphicsDevice.SetRenderTarget(null);

        // Save as PNG
        if (Flags.Read(Flag.SaveAsPng))
            SaveToPng(_spritesRenderTarget);
        
        // UI
        _game.GraphicsDevice.Clear(Color.Black);
        DrawUI();

        // All sprites onto the screen
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_spritesRenderTarget, AppLayout.DrawPos, AppLayout.DrawSize, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        _spriteBatch.End();
    }

    private void SaveToPng(RenderTarget2D saveContent) {
        var saveRT = new RenderTarget2D(_game.GraphicsDevice, 1024, 512);
        _game.GraphicsDevice.SetRenderTarget(saveRT);
        _game.GraphicsDevice.Clear(Color.Transparent);
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(saveContent, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
        _spriteBatch.End();

        _game.GraphicsDevice.SetRenderTarget(null);
        
        using var pngFile = new FileStream("render.png", FileMode.Create);
            saveRT.SaveAsPng(pngFile, 1024, 512);
    }

}