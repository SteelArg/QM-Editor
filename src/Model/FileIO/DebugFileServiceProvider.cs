using System;

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

}