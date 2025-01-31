using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QMEditor.Model;

public class AssetFolder {

    private string _path;
    private string _assetsPath { get => $"assets\\{_path}"; }
    private AssetFactory _factory;
    private Dictionary<string, Asset> _assets;

    public AssetFolder(string path) {
        _path = path;
        _assets = new Dictionary<string, Asset>();
        _factory = new AssetFactory();
    }

    public void ScanAndLoad() {
        Directory.CreateDirectory(_assetsPath);

        _assets.Clear();
        string[] files = ServiceLocator.FileService.GetAllFiles(_assetsPath);
        
        foreach (string file in files) {
            Asset asset = _factory.Create($"{_path}\\{file}");
            if (_assets.ContainsKey(asset.Name)) continue;
            
            asset.Load();
            _assets.Add(asset.Name, asset);
        }
    }

    public Asset GetAsset(string name) => _assets[name];
    public Asset TryGetAsset(string name) => _assets.ContainsKey(name) ? GetAsset(name) : null;

    public Asset[] GetAssets() => _assets.Values.ToArray();

    public Asset[] GetAssetsBySearch(string search = null) {
        if (search == null)
            return GetAssets();

        List<string> searchedAssetNames = new List<string>();

        foreach (string assetName in _assets.Keys) {
            string trimmedName = assetName.Trim().Trim('_').Trim('-');
            if (trimmedName.Contains(search))
                searchedAssetNames.Add(assetName);
        }

        List<Asset> searchedAssets = new List<Asset>();
        foreach (string assetName in searchedAssetNames) {
            searchedAssets.Add(_assets[assetName]);
        }

        return searchedAssets.ToArray();
    }

}