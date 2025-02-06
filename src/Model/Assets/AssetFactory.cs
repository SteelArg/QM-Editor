using System.Collections.Generic;
using System.IO;

namespace QMEditor.Model;

public class AssetFactory {

    public AssetFactory() {}

    public AssetBase Create(string path) {
        string ext = Path.GetExtension(path).ToLower();
        switch (ext) {
            case ".gif":
                return new GifAsset(path);
            default:
                return new StaticAsset(path);
        }
    }

}

public class GroupedAssetsFactory {

    Dictionary<string, List<string>> groupedAssets;

    public GroupedAssetsFactory() {
        groupedAssets = new Dictionary<string, List<string>>();
    }

    public void AddAssetPath(string assetPath) {
        string assetGroup = VariatedAsset.GetNameWithoutVariation(assetPath);
        if (groupedAssets.ContainsKey(assetGroup))
            groupedAssets[assetGroup].Add(assetPath);
        else
            groupedAssets.Add(assetGroup, new List<string>() { assetPath } );
    }

    public AssetBase[] CreateAssets() {
        List<AssetBase> assets = new List<AssetBase>();

        foreach (string group in groupedAssets.Keys) {
            string[] assetPaths = groupedAssets[group].ToArray();
            if (assetPaths.Length > 1) {
                assets.Add(new VariatedAsset(assetPaths));
            }
            else if (Path.GetExtension(assetPaths[0]).ToLower() == ".gif") {
                assets.Add(new GifAsset(assetPaths[0]));
            }
            else {
                assets.Add(new StaticAsset(assetPaths[0]));
            }
        }

        return assets.ToArray();
    }

}