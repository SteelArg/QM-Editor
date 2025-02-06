using System;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class GifAsset : StaticAsset {

    private Texture2D[] _frames;

    public GifAsset(string path) : base(path) {}

    protected override void LoadTextures() {
        _frames = ServiceLocator.FileService.LoadGif(_path);
    }

    protected override Texture2D SelectTexture(int frame, string variation = null) {
        int totalFrames = AppSettings.RenderFrameCount.Value;
        if (_frames.Length*2 <= totalFrames)
            frame = frame % _frames.Length;
        else
            frame = Math.Clamp(frame, 0, _frames.Length-1);
        return _frames[frame];
    }

}