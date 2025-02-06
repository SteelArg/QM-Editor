using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class VariatedAsset : AssetBase {

    Dictionary<string, AssetBase> variations = new Dictionary<string, AssetBase>();

    public VariatedAsset(string[] variationsPath) {
        var assetFactory = new AssetFactory();
        foreach (string variationPath in variationsPath) {
            string[] pathSplit = Path.GetFileNameWithoutExtension(variationPath).Split("__");
            string variationName = pathSplit.Length > 1 ? pathSplit[1] : "base";

            if (variationName == "base")
                SetPathAsName(variationPath);
            
            variations.Add(variationName, assetFactory.Create(variationPath));
        }
    }

    protected override void LoadTextures() {
        foreach (string variation in variations.Keys) {
            variations[variation].Load();
        }
    }

    protected override Texture2D SelectTexture(int frame, string variation) {
        variation = variation ?? "base";
        return variations[variation].GetTexture(frame);
    }

    public string[] GetPossibleVariations() {
        return variations.Keys.ToArray();
    }

    public static string GetVariation(string path) {
        string[] pathSplit = Path.GetFileNameWithoutExtension(path).Split("__");
        string variationName = pathSplit.Length > 1 ? pathSplit[1] : "base";
        return variationName;
    }

    public static string GetNameWithoutVariation(string path) {
        string[] pathSplit = Path.GetFileNameWithoutExtension(path).Split("__");
        return pathSplit[0];
    }

}