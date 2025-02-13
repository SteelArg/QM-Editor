using System;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model.IO;

public class DebugFileServiceProvider : IFileService {

    private IFileService _fileService;

    public DebugFileServiceProvider(IFileService fileService) {
        _fileService = fileService;
    }


    public string Read(string path) {
        Console.WriteLine($"Read from {path}");
        return _fileService.Read(path);
    }

    public void Write(string path, string content) {
        Console.WriteLine($"Write to {path}:\n{content}");
        _fileService.Write(path, content);
    }

    public string[] GetAllFiles(string path, bool fullNames = false) {
        Console.WriteLine($"Got all files in {path}");
        return _fileService.GetAllFiles(path, fullNames);
    }

    public Texture2D LoadTexture(string path) {
        Console.WriteLine($"Loaded texture from {path}");
        return _fileService.LoadTexture(path);
    }

    public Texture2D[] LoadGif(string path) {
        Console.WriteLine($"Loaded textures from {path}");
        return _fileService.LoadGif(path);
    }

    public void SaveAsGif(string path, RenderTarget2D[] renderTargets, int[] gifSize, int frameDelay) {
        Console.WriteLine($"Saved a GIF to {path}");
        _fileService.SaveAsGif(path, renderTargets, gifSize, frameDelay);
    }

}