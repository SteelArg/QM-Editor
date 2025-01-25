using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor.Controllers;

public class AssetsTab : Tab {

    private AssetsLoader _loader;
    private AssetsPreferences _preferences;

    private Dictionary<AssetsFolders, AssetsList> _assetsLists;
    private AssetView _assetView;

    private Asset _selectedAsset;
    private AssetsFolders _selectedAssetFolder;

    public AssetsTab() : base() {
        _loader = new AssetsLoader();
        _preferences = new AssetsPreferences(_loader);
    }

    public override void Load() {
        _loader.Load();

        _assetsLists = new Dictionary<AssetsFolders, AssetsList>();
        string[] assetsListNames = ["Tiles", "Characters", "Accessories", "Props"];
        AssetsFolders[] assetsListFolders = [AssetsFolders.Tiles, AssetsFolders.Characters, AssetsFolders.Accessories, AssetsFolders.Props];
        for (int i = 0; i < 4; i++) AddAssetsList(assetsListNames[i], assetsListFolders[i]);

        _assetView = new AssetView();
        _assetView.PlaceAsset += OnClickedPlaceAsset;

        base.Load();
    }

    private void AddAssetsList(string listName, AssetsFolders folder) {
        AssetsList assetsList = new AssetsList(listName, _loader.GetAllAssetNames(folder), l => l);
        assetsList.AssetSelected += (string name) => { OnAssetSelected(name, folder); };
        _assetsLists.Add(folder, assetsList);
    }

    protected override Widget BuildUI() {
        HorizontalStackPanel mainGrid = new HorizontalStackPanel {
            DefaultProportion = new Proportion(ProportionType.Pixels, 600),
            Spacing = 8, ShowGridLines = true
        };

        foreach (AssetsFolders folder in _assetsLists.Keys) {
            mainGrid.Widgets.Add(_assetsLists[folder].BuildUI());
        }
        mainGrid.Widgets.Add(_assetView.Widget);

        Myra.Graphics2D.UI.Grid.SetColumnSpan(mainGrid, 2);
        return mainGrid;
    }

    public override void Open() {}
    public override void Close() {}
    
    public override void Draw(SpriteBatch spriteBatch) {
        // noop
    }

    public void OnAssetSelected(string assetName, AssetsFolders folder) {
        _selectedAsset = _loader.GetAsset(assetName, folder);
        _selectedAssetFolder = folder;
        _assetView.SetAsset(_selectedAsset.NameOfFile, _selectedAsset.GetTexture());
    }

    public void OnClickedPlaceAsset() {

        switch (_selectedAssetFolder) {
            case AssetsFolders.Tiles:
                _manager.SwitchToTab(1);
                WorldEditor.Instance.SetObjectInCursor(new TileFactory(_selectedAsset));
                return;
            case AssetsFolders.Props:
                _manager.SwitchToTab(1);
                WorldEditor.Instance.SetObjectInCursor(new PropFactory(_selectedAsset));
                return;
        }

        _manager.SwitchToTab(2);
        switch (_selectedAssetFolder) {
            case AssetsFolders.Characters:
                CharacterEditor.Instance.SetCharacterAsset(_selectedAsset);
                break;
            case AssetsFolders.Accessories:
                CharacterEditor.Instance.AddAccessory(_selectedAsset);
                break;
        }
    }

}