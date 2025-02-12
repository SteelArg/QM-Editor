using System;

namespace QMEditor.Model;

[Flags]
public enum AssetsFolders {
    None = 0,
    Tiles = 1,
    Characters = 2,
    Accessories = 4,
    Props = 8,
    All = Tiles | Characters | Accessories | Props
}

public static class AssetsFoldersExtensions {
    public static bool IsFolderSelected(this AssetsFolders assetsFolders, AssetsFolders selectedFolder) {
        return (assetsFolders & selectedFolder) == selectedFolder;
    }
}

public static class AssetsFoldersHelper {
    public static AssetsFolders FoldersByObjectType(object obj) {
        if (obj is Tile)
            return AssetsFolders.Tiles;
        if (obj is Character)
            return AssetsFolders.Characters;
        if (obj is Accessory)
            return AssetsFolders.Accessories;
        if (obj is Prop)
            return AssetsFolders.Props;
        return AssetsFolders.All;
    }
}