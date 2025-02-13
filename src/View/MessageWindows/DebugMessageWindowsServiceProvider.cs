using System;

namespace QMEditor.View;

public class DebugMessageWindowsServiceProvider : IMessageWindowsService {

    private readonly IMessageWindowsService _wrappedService;

    public DebugMessageWindowsServiceProvider(IMessageWindowsService service) {
        _wrappedService = service;
    }

    public void InfoWindow(string infoText) {
        Console.WriteLine($"INFO: {infoText}");
        _wrappedService.InfoWindow(infoText);
    }

    public void ErrorWindow(string errorText) {
        Console.WriteLine($"ERROR: {errorText}");
        _wrappedService.ErrorWindow(errorText);
    }

}