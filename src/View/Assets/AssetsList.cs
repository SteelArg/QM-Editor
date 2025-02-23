using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using QMEditor.Controllers;

namespace QMEditor.View;

public class AssetsList {

    public Action<string> AssetSelected;
    public Action<string> SearchChanged;

    private List<string> _assets;
    private string _title;
    private Func<List<string>, List<string>> _sortAssets;

    private VerticalStackPanel _assetsStack;
    private TextBox _searchBox;

    public AssetsList(string listName, Func<List<string>, List<string>> sortAssets) {
        _title = listName;
        _sortAssets = sortAssets;
    }

    public void SetAssets(List<string> assets) {
        _assets = assets;

        if (_assetsStack == null) return;

        _assetsStack.Widgets.Clear();

        List<string> sortedAssets = _sortAssets.Invoke(_assets);
        foreach (string asset in sortedAssets) {
            _assetsStack.Widgets.Add(BuildButtonForAsset(asset));
        }
    }

    public Widget BuildUI(int gridColumn = 0) {
        _assetsStack = new VerticalStackPanel() {
            Width = 250
        };

        SetAssets(_assets);

        _searchBox = new TextBox {
            HintText = "Search...", Margin = new Myra.Graphics2D.Thickness(10)
        };
        _searchBox.TextChangedByUser += (s, e) => { SearchChanged?.Invoke(_searchBox.Text); };

        var title = new Label {
            Text = _title, Font = FontLoader.GetFont(30, FontLoader.FontType.Bold),
            VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
            TextColor = Color.AliceBlue
        };

        var divider = new HorizontalSeparator();

        var list = new VerticalStackPanel();
        list.Widgets.Add(title);
        list.Widgets.Add(divider);
        list.Widgets.Add(_searchBox);
        list.Widgets.Add(_assetsStack);

        Grid.SetColumn(list, gridColumn);

        return list;
    }

    private Widget BuildButtonForAsset(string assetName) {
        var button = new Button {
            Content = new Label {
                Text = assetName,
                TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                Font = FontLoader.GetFont(20)
            },
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center,
            Width = 500, Margin = new Myra.Graphics2D.Thickness(2),
        };
        button.Click += (s, a) => { AssetSelected?.Invoke(assetName); };
        
        return button;
    }

}