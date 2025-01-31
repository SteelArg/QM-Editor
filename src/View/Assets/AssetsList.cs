using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

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
            HintText = "Search..."
        };
        _searchBox.TextChangedByUser += (s, e) => { SearchChanged?.Invoke(_searchBox.Text); };

        var title = new Label {
            Text = _title,
            VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
            TextColor = Color.AliceBlue
        };

        var list = new VerticalStackPanel();
        list.Widgets.Add(title);
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
            },
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
        };
        button.Click += (s, a) => { AssetSelected?.Invoke(assetName); };
        
        return button;
    }

}