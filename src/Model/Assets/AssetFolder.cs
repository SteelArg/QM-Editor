using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QMEditor.Model;

public class AssetFolder {

    private string _path;
    private string _assetsPath { get => $"assets\\{_path}"; }
    private GroupedAssetsFactory _factory;
    private Dictionary<string, AssetBase> _assets;

    public AssetFolder(string path) {
        _path = path;
        _assets = new Dictionary<string, AssetBase>();
        _factory = new GroupedAssetsFactory();
    }

    public void ScanAndLoad() {
        Directory.CreateDirectory(_assetsPath);

        _assets.Clear();
        string[] files = ServiceLocator.FileService.GetAllFiles(_assetsPath);

        foreach (string file in files) {
            _factory.AddAssetPath($"{_path}\\{file}");
        }
        
        foreach (AssetBase asset in _factory.CreateAssets()) {
            asset.Load();
            _assets.Add(asset.Name, asset);
        }
    }

    public AssetBase GetAsset(string name) => _assets[name];
    public AssetBase TryGetAsset(string name) => _assets.ContainsKey(name) ? GetAsset(name) : null;

    public AssetBase[] GetAssets() => _assets.Values.ToArray();

    public AssetBase[] GetAssetsBySearch(string search = null) {
        if (search == null)
            return GetAssets();

        List<string> searchedAssetNames = new List<string>();

        foreach (string assetName in _assets.Keys) {
            string trimmedName = assetName.Trim().Trim('_').Trim('-');
            if (trimmedName.Contains(search))
                searchedAssetNames.Add(assetName);
        }

        List<AssetBase> searchedAssets = new List<AssetBase>();
        foreach (string assetName in searchedAssetNames) {
            searchedAssets.Add(_assets[assetName]);
        }

        return searchedAssets.ToArray();
    }

}