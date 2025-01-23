using System;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class GifAsset : Asset {

    private Texture2D[] _frames;

    public GifAsset(string path) : base(path) {}

    public override void Load() {
        _frames = ServiceLocator.FileService.LoadGif(_path);
        _isLoaded = true;
    }

    public override Texture2D GetTexture(int frame) {
        if (!_isLoaded) return null;
        frame = Math.Clamp(frame, 0, _frames.Length-1);
        return _frames[frame];
    }

    public override int[] GetSize() {
        if (!_isLoaded) return [0,0];
        return [_frames[0].Width, _frames[0].Height];
    }

}