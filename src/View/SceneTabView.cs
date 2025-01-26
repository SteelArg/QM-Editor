using System;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

public class SceneTabView {

    public FrameLooperView FrameLooperView { get => _frameLooperView; }
    public Action RenderClicked;

    private FrameLooperView _frameLooperView;
    private VerticalStackPanel _stackPanel;

    public SceneTabView() {
        _frameLooperView = new FrameLooperView();
    }

    public Widget BuildUI() {
        _stackPanel = new VerticalStackPanel() {
            Spacing = 20, VerticalAlignment = VerticalAlignment.Bottom
        };
        Grid.SetColumn(_stackPanel, 1);

        _stackPanel.Widgets.Add(_frameLooperView.BuildUI());
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