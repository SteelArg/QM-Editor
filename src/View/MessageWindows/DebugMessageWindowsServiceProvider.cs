namespace QMEditor.View;

public class DebugMessageWindowsServiceProvider : IMessageWindowsService {

    private readonly IMessageWindowsService _wrappedService;

    public DebugMessageWindowsServiceProvider(IMessageWindowsService service) {
        _wrappedService = service;
    }

    public void InfoWindow(string infoText) {
        ServiceLocator.LoggerService.Log($"Message window INFO:\n{infoText}");
        _wrappedService.InfoWindow(infoText);
    }

    public void ErrorWindow(string errorText) {
        ServiceLocator.LoggerService.Log($"Message window ERROR:\n{errorText}");
        _wrappedService.ErrorWindow(errorText);
    }

}