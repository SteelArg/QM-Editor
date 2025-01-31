using System;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using QMEditor.View;

public class SceneTabView {

    public FrameLooperWidget FrameLooper { get => _frameLooper; }
    public Action RenderClicked;

    private FrameLooperWidget _frameLooper;
    private VerticalStackPanel _stackPanel;

    public SceneTabView() {}

    public Widget BuildUI() {
        _stackPanel = new VerticalStackPanel() {
            Spacing = 20, VerticalAlignment = VerticalAlignment.Bottom
        };
        Grid.SetColumn(_stackPanel, 1);

        _frameLooper = new FrameLooperWidget();

        _stackPanel.Widgets.Add(_frameLooper);
        _stackPanel.Widgets.Add(BuildRenderButton());

        return _stackPanel;
    }

    private Widget BuildRenderButton() {
        var renderButton = new Button() {
            Content = new Label() {
                Text = "Render", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom,
            Width = 200, Height = 120,
            Margin = new Thickness(20)
        };
        renderButton.Click += (s, e) => { RenderClicked?.Invoke(); };
        
        return renderButton;
    }

}