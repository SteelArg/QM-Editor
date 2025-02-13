using Myra.Graphics2D.UI;

namespace QMEditor.View;

public class DefaultMessageWindowsServiceProvider : IMessageWindowsService {

    public void InfoWindow(string infoText) {
        Window infoWindow = BuildWindow("Info", infoText);
        infoWindow.ShowModal(Global.Desktop);
    }

    public void ErrorWindow(string errorText) {
        Window errorWindow = BuildWindow("Error", errorText);
        errorWindow.ShowModal(Global.Desktop);
    }

    private Window BuildWindow(string title, string text, string buttonText="Ok") {
        var stack = new VerticalStackPanel { Spacing = 10 };

        var window = new Window {
            Title = title,
            Content = stack,
        };
        
        stack.Widgets.Add(new Label { 
            Text = text,
        });

        var okButton = new Button {
            Content = new Label {
                Text=buttonText, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            HorizontalAlignment = HorizontalAlignment.Center, Height= 30, Width = 60
        };
        okButton.Click += (s, e) => { window.Close(); };
        stack.Widgets.Add(okButton);

        return window;
    }

}