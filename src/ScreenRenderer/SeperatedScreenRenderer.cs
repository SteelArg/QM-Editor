using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor;

public class SeperatedScreenRenderer : ScreenRenderer {

    public SeperatedScreenRenderer(int[] windowSize) : base(windowSize) {}

    public override void Draw(GameTime gameTime) {
        // Render all sprites
        RenderTarget2D spritesRT = SpriteRenderList.RenderToTarget(AppSettings.RenderOutputSize.Get());
        
        // UI
        Global.Game.GraphicsDevice.SetRenderTarget(null);
        Global.Game.GraphicsDevice.Clear(Color.Transparent);
        UIRenderList.Render();

        // All sprites fit into empty space

        // BlendState bs = new BlendState() {
        //     ColorSourceBlend = Blend.InverseSourceAlpha, AlphaSourceBlend = Blend.One,
        //     ColorDestinationBlend = Blend.One, AlphaDestinationBlend = Blend.One,
        // };

        _spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.Additive, samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(spritesRT, AppLayout.DrawPos, null, Color.White, 0f, Vector2.Zero, CalculateScale(), SpriteEffects.None, 0f);
        _spriteBatch.End();

        spritesRT.Dispose();
    }

    public override Vector2 GetMousePosition() {
        Vector2 pos =  base.GetMousePosition();
        pos -= new Vector2(0f, AppLayout.TabSelectHeight);
        pos /= CalculateScale();
        return pos;
    }

    private float CalculateScale() {
        float originalXSize = AppSettings.RenderOutputSize.Get()[0];
        float currentXSize = AppLayout.DrawSize.X;
        float scale = currentXSize / originalXSize;
        return scale;
    }

}