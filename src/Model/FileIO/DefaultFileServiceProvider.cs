using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using ImageMagick;

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

    public Texture2D[] LoadGif(string path) {
        if (Path.GetExtension(path).ToLower() != ".gif") {
            Console.WriteLine("TRIED TO LOAD GIF FROM A FILE THAT IS NOT A GIF");
            return null;
        }

        using var framesCollection = new MagickImageCollection(path, MagickFormat.Gif);
        framesCollection.Coalesce();

        int frames = framesCollection.Count;
        Texture2D[] loadedFrames = new Texture2D[frames];
        
        for (int i = 0; i < frames; i++) {
            // Get frame
            IMagickImage frame = framesCollection[i];
            using var frameStream = new MemoryStream();
            frame.Write(frameStream);
            
            // Load to texture
            frameStream.Seek(0, SeekOrigin.Begin);
            loadedFrames[i] = Texture2D.FromStream(Global.Game.GraphicsDevice, frameStream);
        }
        return loadedFrames;
    }
    
    public void SaveAsGif(string path, RenderTarget2D[] renderTargets, int[] gifSize, int frameDelay) {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        Directory.CreateDirectory(Path.GetDirectoryName(path) + "\\temp");

        var debugTimer = new DebugTimer();

        var gifCollection = new MagickImageCollection();
        // For some FUCKING reason ImageMagick has 10x of the frame delay in the GIF
        uint gifFrameDelay = (uint)frameDelay/10;

        for (int i = 0; i < renderTargets.Length; i++) {
            var pngStream = new MemoryStream();
            renderTargets[i].SaveAsPng(pngStream, renderTargets[i].Width, renderTargets[i].Height);
            pngStream.Seek(0, SeekOrigin.Begin);
            using FileStream fileStream = File.Create($"{Path.GetDirectoryName(path)}\\temp\\frame{i}.png");
                pngStream.CopyTo(fileStream);
            pngStream.Seek(0, SeekOrigin.Begin);

            var frame = new MagickImage(pngStream) {
                AnimationDelay = gifFrameDelay, GifDisposeMethod = GifDisposeMethod.Background
            };
            gifCollection.Add(frame);
            pngStream.Dispose();
        }

        gifCollection.Write(path);
        gifCollection.Dispose();

        debugTimer.Timestamp("Create GIF");
        debugTimer.LogAll(false);
    }

}