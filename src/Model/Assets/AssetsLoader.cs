using System.Collections.Generic;

namespace QMEditor.Model;

public class AssetsLoader : Singleton<AssetsLoader> {

    private AssetFolder _tiles;
    private AssetFolder _characters;
    private AssetFolder _accessories;

    public AssetsLoader() {
        _tiles = new AssetFolder("tiles");
        _characters = new AssetFolder("characters");
        _accessories = new AssetFolder("accessories");
    }

    public void Load() {
        _tiles.Scan();
        _characters.Scan();
        _accessories.Scan();
    }

    public Asset GetAsset(string assetName, AssetsFolders folders) {
        if (folders.IsFolderSelected(AssetsFolders.Tiles))
            return _tiles.GetAsset(assetName);
        if (folders.IsFolderSelected(AssetsFolders.Characters))
            return _characters.GetAsset(assetName);
        if (folders.IsFolderSelected(AssetsFolders.Accessories))
            return _accessories.GetAsset(assetName);
        return null;
    } 

    public List<string> GetAllAssetNames(AssetsFolders folders) {
        List<string> allAssets = new List<string>();
        if (folders.IsFolderSelected(AssetsFolders.Tiles)) {
            foreach (Asset asset in _tiles.GetAssets()) {
                allAssets.Add(asset.Name);
            }
        }
        if (folders.IsFolderSelected(AssetsFolders.Characters)) {
            foreach (Asset asset in _characters.GetAssets()) {
                allAssets.Add(asset.Name);
            }
        }
        if (folders.IsFolderSelected(AssetsFolders.Accessories)) {
            foreach (Asset asset in _accessories.GetAssets()) {
                allAssets.Add(asset.Name);
            }
        }
        return allAssets;
    }

    public Tile GetTile(string tileName) {
        return new Tile(_tiles.GetAsset(tileName));
    }

    public Character GetCharacter(string characterName) {
        return new Character(_characters.GetAsset(characterName));
    }

    public Accessory GetAccessory(string accessoryName) {
        return new Accessory(_accessories.GetAsset(accessoryName));
    }

}