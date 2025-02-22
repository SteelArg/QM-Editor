using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QMEditor.Controllers;

namespace QMEditor;

public class ScreenRenderer : Singleton<ScreenRenderer> {

    public RenderList UIRenderList;
    public RenderList SpriteRenderList;

    private GraphicsDeviceManager _graphics;
    protected SpriteBatch _spriteBatch;

    protected int[] _windowSize;

    public ScreenRenderer(int[] windowSize) {
        _graphics = new GraphicsDeviceManager(Global.Game);
        _windowSize = windowSize;
    }

    public void Init() {
        _graphics.PreferredBackBufferWidth = _windowSize[0];
        _graphics.PreferredBackBufferHeight = _windowSize[1];
        _graphics.ApplyChanges();
        UIRenderList = new RenderList();
        SpriteRenderList = new RenderList();
    }

    public virtual void Load() {
        _spriteBatch = new SpriteBatch(Global.Game.GraphicsDevice);
    }

    public virtual void Draw(GameTime gameTime) {
        RenderTarget2D spritesRT = SpriteRenderList.RenderToTarget(AppSettings.RenderOutputSize.Get());
        Global.Game.GraphicsDevice.SetRenderTarget(null);
        Global.Game.GraphicsDevice.Clear(Color.Black);
        
        UIRenderList.Render();

        var bs = new BlendState();
        bs.ColorSourceBlend = Blend.SourceAlpha;
        bs.AlphaSourceBlend = Blend.One;
        bs.ColorDestinationBlend = Blend.InverseSourceAlpha;
        bs.AlphaDestinationBlend = Blend.InverseSourceAlpha;
        
        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        _spriteBatch.Draw(spritesRT, AppLayout.DrawPos, Color.White);
        _spriteBatch.End();

        spritesRT.Dispose();
    }

    public virtual Vector2 GetMousePosition() {
        return Mouse.GetState().Position.ToVector2();
    }

}