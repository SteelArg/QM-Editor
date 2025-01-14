using System.IO;

namespace QMEditor.Model;

public class AssetFactory {

    public AssetFactory() {}

    public Asset Create(string path) {
        string ext = Path.GetExtension(path);
        switch (ext.ToLower()) {
            case ".gif":
                return new GifAsset(path);
            default:
                return new Asset(path);
        }
    }

}