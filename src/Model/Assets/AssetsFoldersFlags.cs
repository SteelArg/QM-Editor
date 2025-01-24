using System;

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