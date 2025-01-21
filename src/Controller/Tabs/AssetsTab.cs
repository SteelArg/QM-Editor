using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor.Controllers;

public class AssetsTab : Tab {

    private AssetsLoader _loader;
    private AssetsPreferences _preferences;

    private AssetsList _tilesList;
    private AssetsList _charactersList;
    private AssetsList _accessoriesList;
    private AssetView _assetView;

    private Asset _selectedAsset;
    private AssetsFolders _selectedAssetFolder;

    public AssetsTab() : base() {
        _loader = new AssetsLoader();
        _preferences = new AssetsPreferences(_loader);
    }

    public override void Load() {
        _loader.Load();

        _tilesList = new AssetsList("Tiles", _loader.GetAllAssetNames(AssetsFolders.Tiles), l => l);
        _charactersList = new AssetsList("Characters", _loader.GetAllAssetNames(AssetsFolders.Characters), l => l);
        _accessoriesList = new AssetsList("Accessories", _loader.GetAllAssetNames(AssetsFolders.Accessories), l => l);

        _tilesList.AssetSelected += (string name) => { OnAssetSelected(name, AssetsFolders.Tiles); };
        _charactersList.AssetSelected += (string name) => { OnAssetSelected(name, AssetsFolders.Characters); };
        _accessoriesList.AssetSelected += (string name) => { OnAssetSelected(name, AssetsFolders.Accessories); };

        _assetView = new AssetView();
        _assetView.PlaceAsset += OnClickedPlaceAsset;

        base.Load();
    }

    protected override Widget BuildUI() {
        HorizontalStackPanel mainGrid = new HorizontalStackPanel {
            DefaultProportion = new Proportion(ProportionType.Pixels, 600),
            Spacing = 8, ShowGridLines = true
        };

        mainGrid.Widgets.Add(_tilesList.BuildUI());
        mainGrid.Widgets.Add(_charactersList.BuildUI());
        mainGrid.Widgets.Add(_accessoriesList.BuildUI());
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
        if (_selectedAssetFolder == AssetsFolders.Tiles) {
            _manager.SwitchToTab(1);
            WorldEditor.ObjectInCursor = new TileFactory(_selectedAsset);
            return;
        }

        _manager.SwitchToTab(2);
        CharacterTab characterTab = (CharacterTab)_manager.CurrentTab;
        switch (_selectedAssetFolder) {
            case AssetsFolders.Characters:
                characterTab.SelectCharacter(_selectedAsset);
                break;
            case AssetsFolders.Accessories:
                characterTab.SelectAccessory(_selectedAsset);
                break;
        }
    }

}