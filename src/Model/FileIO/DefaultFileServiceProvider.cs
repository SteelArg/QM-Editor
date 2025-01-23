using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model.IO;

public class DefaultFileServiceProvider : IFileService {


    public string Read(string path) {
        if (!File.Exists(path)) return string.Empty;
        return File.ReadAllText(path);
    }

    public void Write(string path, string content) {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, content);
    }

    public string[] GetAllFiles(string path, bool fullNames = false) {
        string[] fullFilenames = Directory.GetFiles(path);
        if (fullNames)
            return fullFilenames;

        string[] shortFilenames = new string[fullFilenames.Length];
        for (int i = 0; i < fullFilenames.Length; i++) {
            shortFilenames[i] = Path.GetFileName(fullFilenames[i]);
        }
        return shortFilenames;
    }

    public Texture2D LoadTexture(string path) {
        var fileStream = new FileStream(path, FileMode.Open);
        return Texture2D.FromStream(Global.Game.GraphicsDevice, fileStream);
    }

    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public Texture2D[] LoadGif(string path) {
        if (Path.GetExtension(path).ToLower() != ".gif") {
            Console.WriteLine("TRIED TO LOAD GIF FROM A FILE THAT IS NOT A GIF");
            return null;
        }

        Image gifImage = Image.FromFile(path);
        int frames = gifImage.GetFrameCount(FrameDimension.Time);
        Texture2D[] loadedFrames = new Texture2D[frames];
        
        for (int i = 0; i < frames; i++) {
            // Get frame
            gifImage.SelectActiveFrame(FrameDimension.Time, i);
            Image frameImage = (Image)gifImage.Clone();
            
            // Load to texture
            var stream = new MemoryStream();
            frameImage.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            loadedFrames[i] = Texture2D.FromStream(Global.Game.GraphicsDevice, stream);
        }
        return loadedFrames;
    }
    
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public void SaveAsGif(string path, RenderTarget2D[] renderTargets, int[] gifSize, int frameDelay) {
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        using (var gif = AnimatedGif.AnimatedGif.Create(path, frameDelay)) {
            for (int i = 0; i < renderTargets.Length; i++) {
                var pngStream = new MemoryStream();
                renderTargets[i].SaveAsPng(pngStream, gifSize[0], gifSize[1]);
                pngStream.Seek(0, SeekOrigin.Begin);
                var pngImage = Image.FromStream(pngStream);
                pngStream.Close();
                gif.AddFrame(pngImage, delay: frameDelay, quality: AnimatedGif.GifQuality.Bit8);
            }
        }
    }

}