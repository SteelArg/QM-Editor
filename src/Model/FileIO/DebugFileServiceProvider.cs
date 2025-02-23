using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model.IO;

public class DebugFileServiceProvider : IFileService {

    private IFileService _fileService;

    public DebugFileServiceProvider(IFileService fileService) {
        _fileService = fileService;
    }


    public string Read(string path) {
        ServiceLocator.LoggerService.Log($"Read from {path}.");
        return _fileService.Read(path);
    }

    public void Write(string path, string content) {
        ServiceLocator.LoggerService.Log($"Write to {path}:\n{content}.");
        _fileService.Write(path, content);
    }

    public string[] GetAllFiles(string path, bool fullNames = false) {
        ServiceLocator.LoggerService.Log($"Got all files in {path}.");
        return _fileService.GetAllFiles(path, fullNames);
    }

    public Texture2D LoadTexture(string path) {
        ServiceLocator.LoggerService.Log($"Loaded texture from {path}.");
        return _fileService.LoadTexture(path);
    }

    public Texture2D[] LoadGif(string path) {
        ServiceLocator.LoggerService.Log($"Loaded textures from {path}.");
        return _fileService.LoadGif(path);
    }

    public void SaveAsGif(string path, RenderTarget2D[] renderTargets, int[] gifSize, int frameDelay) {
        ServiceLocator.LoggerService.Log($"Saved a GIF to {path}.");
        _fileService.SaveAsGif(path, renderTargets, gifSize, frameDelay);
    }

}