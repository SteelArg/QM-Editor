using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor;

public class SeperatedScreenRenderer : ScreenRenderer {

    private RenderTarget2D _spritesRenderTarget;

    private float _scale;

    public SeperatedScreenRenderer(int[] windowSize, float scale = 1f) : base(windowSize) {
        _scale = scale;
    }

    public override void Load() {
        base.Load();
        _spritesRenderTarget = new RenderTarget2D(Global.Game.GraphicsDevice, _windowSize[0], _windowSize[1]);
    }

    public override void Draw(GameTime gameTime) {
        // Sprites to seperate render target
        Global.Game.GraphicsDevice.SetRenderTarget(_spritesRenderTarget);
        Global.Game.GraphicsDevice.Clear(Color.Transparent);
        DrawSprites();
        Global.Game.GraphicsDevice.SetRenderTarget(null);
        
        // UI
        Global.Game.GraphicsDevice.Clear(Color.Transparent);
        DrawUI();

        // All sprites onto the screen
        _spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_spritesRenderTarget, AppLayout.DrawPos, AppLayout.DrawSize, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        _spriteBatch.End();
    }

    public override Vector2 GetMousePosition() {
        Vector2 pos =  base.GetMousePosition();
        pos -= new Vector2(0f, AppLayout.TabSelectHeight);
        pos /= _scale;
        return pos;
    }

}