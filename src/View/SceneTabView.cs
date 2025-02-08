using System;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using QMEditor.View;

public class SceneTabView {

    public FrameLooperWidget FrameLooper { get => _frameLooper; }
    public Action RenderClicked;
    public Action RenderSettingsClicked;

    private FrameLooperWidget _frameLooper;
    private VerticalStackPanel _stackPanel;

    public SceneTabView() {}

    public Widget BuildUI() {
        _stackPanel = new VerticalStackPanel {
            Spacing = 20, VerticalAlignment = VerticalAlignment.Bottom
        };
        Grid.SetColumn(_stackPanel, 1);

        _frameLooper = new FrameLooperWidget();

        var renderButtons = new HorizontalStackPanel {
            Spacing = 5, ShowGridLines = false,
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(20)
        };
        renderButtons.Widgets.Add(BuildButton("Render", ()=>RenderClicked?.Invoke()));
        renderButtons.Widgets.Add(BuildButton("...", ()=>RenderSettingsClicked?.Invoke(), 80, 80));

        _stackPanel.Widgets.Add(_frameLooper);
        _stackPanel.Widgets.Add(renderButtons);

        return _stackPanel;
    }

    private Button BuildButton(string text, Action clickHandler, int width = 160, int height = 80) {
        var button = new Button() {
            Content = new Label() {
                Text = text, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom,
            Width = width, Height = height
        };
        button.Click += (s, e) => clickHandler();
        
        return button;
    }

}