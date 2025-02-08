using System.Collections.Generic;

namespace QMEditor.Model;

public class AssetsLoader : Singleton<AssetsLoader> {

    private Dictionary<AssetsFolders, AssetFolder> _folders;

    public AssetsLoader() {
        _folders = new Dictionary<AssetsFolders, AssetFolder> {
            { AssetsFolders.Tiles, new AssetFolder("tiles") },
            { AssetsFolders.Characters, new AssetFolder("characters") },
            { AssetsFolders.Accessories, new AssetFolder("accessories") },
            { AssetsFolders.Props, new AssetFolder("props") }
        };
    }

    public void Load() {
        foreach (AssetFolder folder in _folders.Values) {
            folder.ScanAndLoad();
        }
    }

    public AssetBase GetAsset(string assetName, AssetsFolders foldersToSearch) {
        AssetBase foundAsset = null;
        foreach (AssetsFolders scannedFolder in _folders.Keys) {
            if (foldersToSearch.IsFolderSelected(scannedFolder))
                foundAsset = foundAsset ?? _folders[scannedFolder].TryGetAsset(assetName);
        }
        return foundAsset;
    }

    public AssetBase GetAnyAsset(AssetsFolders foldersToSearch) {
        foreach (AssetsFolders scannedFolder in _folders.Keys) {
            if (!foldersToSearch.IsFolderSelected(scannedFolder)) continue;
            AssetBase[] assets = _folders[AssetsFolders.Tiles].GetAssets();
            if (assets.Length > 0) return assets[0];
        }
        return null;
    }

    public List<string> GetAllAssetNames(AssetsFolders foldersToSearch, string searchString = null) {
        List<string> allAssets = new List<string>();
        foreach (AssetsFolders scannedFolder in _folders.Keys) {
            if (!foldersToSearch.IsFolderSelected(scannedFolder)) continue;
            foreach (AssetBase asset in _folders[scannedFolder].GetAssetsBySearch(searchString)) {
                allAssets.Add(asset.Name);
            }
        }
        return allAssets;
    }

}