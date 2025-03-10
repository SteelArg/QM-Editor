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

    private AssetBase _selectedAsset;
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
        AssetsList assetsList = new AssetsList(listName, l => l);
        assetsList.SetAssets(_loader.GetAllAssetNames(folder));
        assetsList.AssetSelected += (string name) => { OnAssetSelected(name, folder); };
        assetsList.SearchChanged += (string search) => { assetsList.SetAssets(_loader.GetAllAssetNames(folder, search)); } ;
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
    
    public override RenderTarget2D Draw(SpriteBatch spriteBatch) {
        return null;
    }

    public void OnAssetSelected(string assetName, AssetsFolders folder) {
        _selectedAsset = _loader.GetAsset(assetName, folder);
        _selectedAssetFolder = folder;
        _assetView.SetAsset(_selectedAsset.NameOfFile, _selectedAsset.GetTexture());
    }

    public void OnClickedPlaceAsset() {
        switch (_selectedAssetFolder) {
            case AssetsFolders.Tiles:
                _manager.SwitchToTab<SceneTab>();
                World.Cursor.SetObject(new Tile(_selectedAsset));
                return;
            case AssetsFolders.Props:
                _manager.SwitchToTab<SceneTab>();
                World.Cursor.SetObject(new Prop(_selectedAsset));
                return;
        }

        _manager.SwitchToTab<CharacterTab>();
        switch (_selectedAssetFolder) {
            case AssetsFolders.Characters:
                _manager.GetTab<CharacterTab>().SetCharacterAsset(_selectedAsset);
                break;
            case AssetsFolders.Accessories:
                _manager.GetTab<CharacterTab>().AddAccessoryAsset(_selectedAsset);
                break;
        }
    }

}