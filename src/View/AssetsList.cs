using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myra.Events;
using Myra.Graphics2D.UI;

namespace QMEditor.View;

public class AssetsList {

    public Action<string> AssetSelected;

    private List<string> _assets;
    private string _title;
    private Func<List<string>, List<string>> _sortAssets;

    private VerticalStackPanel _assetsStack;
    private TextBox _searchBox;

    public AssetsList(string listName, List<string> assets, Func<List<string>, List<string>> sortAssets) {
        _assets = assets;
        _title = listName;
        _sortAssets = sortAssets;
    }

    public Widget BuildUI(int gridColumn = 0) {
        _assetsStack = new VerticalStackPanel() {
            Width = 250
        };
        
        List<string> sortedAssets = _sortAssets.Invoke(_assets);
        foreach (string asset in sortedAssets) {
            _assetsStack.Widgets.Add(BuildButtonForAsset(asset));
        }

        _searchBox = new TextBox {
            HintText = "Search..."
        };
        _searchBox.TextChangedByUser += OnSearchTextChanged;

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

    private void OnSearchTextChanged(object sender, ValueChangedEventArgs<string> newSearch) {
        // TODO: Search with new text
    }

}