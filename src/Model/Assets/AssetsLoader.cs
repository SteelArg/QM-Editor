using System;
using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class AssetsLoader : Singleton<AssetsLoader> {

    private AssetFolder _tiles;
    private AssetFolder _characters;

    public AssetsLoader() {
        _tiles = new AssetFolder("tiles");
        _characters = new AssetFolder("characters");
    }

    public void Load() {
        _tiles.Scan();
        _characters.Scan();
    }

    public Asset GetAsset(string assetName, Folders folders) {
        if ((folders & Folders.Tiles) == Folders.Tiles)
            return _tiles.GetAsset(assetName);
        if ((folders & Folders.Characters) == Folders.Characters)
            return _characters.GetAsset(assetName);
        return null;
    } 

    public Tile GetTile(string tileName) {
        return new Tile(_tiles.GetAsset(tileName));
    }

    public Character GetCharacter(string characterName) {
        return new Character(_characters.GetAsset(characterName));
    }

    [Flags]
    public enum Folders {
        None = 0,
        Tiles = 1,
        Characters = 2,
        All = Tiles | Characters
    }

}