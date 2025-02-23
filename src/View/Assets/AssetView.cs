using System;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using QMEditor.Controllers;

namespace QMEditor.View;

public class AssetView {

    public Widget Widget { get => _stack; }
    public Action PlaceAsset;

    private string _assetName;
    private Texture2D _assetTexture;

    private VerticalStackPanel _stack;

    public AssetView() {
        _stack = new VerticalStackPanel();
    }

    public void SetAsset(string name, Texture2D texture) {
        _assetName = name;
        _assetTexture = texture;
        RebuildUI();
    }

    private void RebuildUI() {
        var name = new Label {
            Text = _assetName,
            TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
        };

        var image = new Image {
            Renderable = new TextureRegion(_assetTexture),
            ResizeMode = ImageResizeMode.Stretch,
            Width = 200, Height = (int)((float)_assetTexture.Height/_assetTexture.Width*200f)
        };

        var button = new Button {
            Content = new Label {
                Text = "Place", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center, Font = FontLoader.GetFont(25),
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center,
            Width = 100, Height = 50, Margin = new Myra.Graphics2D.Thickness(10)
        };
        button.Click += (s, e) => { PlaceAsset?.Invoke(); };

        _stack.Widgets.Clear();
        _stack.Widgets.Add(name);
        _stack.Widgets.Add(button);
        _stack.Widgets.Add(image);
    }

}