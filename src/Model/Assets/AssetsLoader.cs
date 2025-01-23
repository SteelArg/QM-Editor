using System.Collections.Generic;

namespace QMEditor.Model;

public class AssetsLoader : Singleton<AssetsLoader> {

    private Dictionary<AssetsFolders, AssetFolder> _folders;

    public AssetsLoader() {
        _folders = new Dictionary<AssetsFolders, AssetFolder> {
            { AssetsFolders.Tiles, new AssetFolder("tiles") },
            { AssetsFolders.Characters, new AssetFolder("characters") },
            { AssetsFolders.Accessories, new AssetFolder("accessories") }
        };
    }

    public void Load() {
        foreach (AssetFolder folder in _folders.Values) {
            folder.ScanAndLoad();
        }
    }

    public Asset GetAsset(string assetName, AssetsFolders foldersToSearch) {
        Asset foundAsset = null;
        foreach (AssetsFolders scannedFolder in _folders.Keys) {
            if (foldersToSearch.IsFolderSelected(scannedFolder))
                foundAsset = foundAsset ?? _folders[scannedFolder].TryGetAsset(assetName);
        }
        return foundAsset;
    } 

    public List<string> GetAllAssetNames(AssetsFolders foldersToSearch) {
        List<string> allAssets = new List<string>();
        foreach (AssetsFolders scannedFolder in _folders.Keys) {
            if (!foldersToSearch.IsFolderSelected(scannedFolder)) continue;
            foreach (Asset asset in _folders[scannedFolder].GetAssets()) {
                allAssets.Add(asset.Name);
            }
        }
        return allAssets;
    }

}