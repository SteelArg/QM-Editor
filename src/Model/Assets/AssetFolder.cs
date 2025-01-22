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

    public void Scan() {
        Directory.CreateDirectory(_assetsPath);

        _assets.Clear();
        string[] files = ServiceLocator.FileService.GetAllFiles(_assetsPath);
        
        foreach (string file in files) {
            Asset asset = _factory.Create($"{_path}\\{file}");
            asset.Load();
            _assets.Add(asset.Name, asset);
        }
    }

    public Asset GetAsset(string name) => _assets[name];
    public Asset TryGetAsset(string name) => _assets.ContainsKey(name) ? GetAsset(name) : null;

    public Asset[] GetAssets() => _assets.Values.ToArray();

}