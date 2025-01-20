using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using QMEditor.View;

namespace QMEditor.Controllers;

public class SceneTab : Tab {

    public SceneTab() : base() {
        
    }

    protected override Widget BuildUI() {
        var renderButton = new Button() {
            Content = new Label() {
                Text = "Render", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom,
            Width = 200, Height = 120,
            Margin = new Thickness(20)
        };
        renderButton.Click += (s, a) => { WorldRenderer.SaveToPng("output\\render.png", [1024, 512]); };

        Grid.SetColumn(renderButton, 1);
        return renderButton;
    }

    public override void Open() {}
    public override void Close() {}

    public override void Update(GameTime gameTime) {
        if (Input.MouseButtonClicked(0))
            WorldEditor.PlaceObjectOnCursor();
        if (Input.MouseButtonHeld(1))
            WorldEditor.ClearCellOnCursor(Input.KeyHeld(Keys.LeftShift));
    }

    public override void Draw(SpriteBatch spriteBatch) {
        WorldRenderer.Render(spriteBatch, 1f);
    }
}