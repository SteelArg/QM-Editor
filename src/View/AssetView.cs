using System;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;

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
            ResizeMode = ImageResizeMode.KeepAspectRatio,
            Width = 200, Height = 200
        };

        var button = new Button {
            Content = new Label {
                Text = "Place", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center
            },
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
        };
        button.Click += (s, e) => { PlaceAsset?.Invoke(); };

        _stack.Widgets.Clear();
        _stack.Widgets.Add(name);
        _stack.Widgets.Add(image);
        _stack.Widgets.Add(button);
    }

}