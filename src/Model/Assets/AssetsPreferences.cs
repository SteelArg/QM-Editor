using System.Collections.Generic;

namespace QMEditor.Model;

public class AssetsPreferences {

    private const string PreferencesPath = "assets\\settings\\preferences.txt";

    private AssetsLoader _loader;
    private StringDataParser _dataParser;

    private List<string> _likedTiles;
    private List<string> _likedCharacters;
    private List<string> _likedAccessories;

    public AssetsPreferences(AssetsLoader loader) {
        _loader = loader;
        _dataParser = new StringDataParser(PreferencesPath);
    }

    public void Load() {
        _dataParser.Load();
        _likedTiles = new List<string>(_dataParser.GetValue("liked_tiles").Split(";"));
        _likedCharacters = new List<string>(_dataParser.GetValue("liked_characters").Split(";"));
        _likedAccessories = new List<string>(_dataParser.GetValue("liked_accessoriess").Split(";"));
    }

    public void AddToPreferences(string assetName, AssetsFolders folder) => AssetInPreferences(assetName, folder, true);
    public void RemoveFromPreferences(string assetName, AssetsFolders folder) => AssetInPreferences(assetName, folder, false);

    public void ClearAllPreferences() {
        _likedTiles = new List<string>();
        _likedCharacters = new List<string>();
        _likedAccessories = new List<string>();
        Save();
    }

    private void AssetInPreferences(string assetName, AssetsFolders folder, bool add) {
        switch (folder) {
            case AssetsFolders.Tiles: 
                if (!add) _likedTiles.Remove(assetName);
                else _likedTiles.Add(assetName);
                break;
            case AssetsFolders.Characters:
                if (!add) _likedCharacters.Remove(assetName);
                else _likedCharacters.Add(assetName);
                break;
            case AssetsFolders.Accessories:
                if (!add) _likedAccessories.Remove(assetName);
                else _likedAccessories.Add(assetName);
                break;
        }
        Save();
    }


    private bool IsAssetLiked(string assetName, AssetsFolders folders) {
        switch (folders) {
            case AssetsFolders.Tiles:
                return _likedTiles.Contains(assetName);
            case AssetsFolders.Characters:
                return _likedCharacters.Contains(assetName);
            case AssetsFolders.Accessories:
                return _likedAccessories.Contains(assetName);
            default:
                return false;
        }
    }

    private void Save() {
        _dataParser.SetValue("liked_tiles", string.Join(";", _likedTiles.ToArray()));
        _dataParser.SetValue("liked_characters", string.Join(";", _likedCharacters.ToArray()));
        _dataParser.SetValue("liked_accessoriess", string.Join(";", _likedAccessories.ToArray()));
        _dataParser.Save();
    }

}