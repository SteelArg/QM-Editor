using System;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace QMEditor.View;

public class SceneTabView {

    public FrameLooperWidget FrameLooper { get => _frameLooper; }
    public ShaderSelectionWidget ShaderSelection { get => _shaderSelection; }
    public Action RenderClicked;
    public Action RenderSettingsClicked;

    private FrameLooperWidget _frameLooper;
    private ShaderSelectionWidget _shaderSelection;
    private VerticalStackPanel _stackPanel;

    public SceneTabView() {}

    public Widget BuildUI(Widget inspectorWidget) {
        var mainGrid = new Grid() {
            DefaultRowProportion = Proportion.Fill
        };
        mainGrid.RowsProportions.Add(Proportion.Auto);
        Grid.SetColumn(mainGrid, 1);

        _stackPanel = new VerticalStackPanel {
            Spacing = 20, VerticalAlignment = VerticalAlignment.Bottom
        };
        Grid.SetRow(_stackPanel, 1);

        var frameLooperAndShaderButtonStack = new HorizontalStackPanel() { Spacing = 20 };

        _frameLooper = new FrameLooperWidget();
        _shaderSelection = new ShaderSelectionWidget();
        frameLooperAndShaderButtonStack.Widgets.Add(_frameLooper);
        frameLooperAndShaderButtonStack.Widgets.Add(_shaderSelection);

        var renderButtons = new HorizontalStackPanel {
            Spacing = 5, ShowGridLines = false,
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(20)
        };
        renderButtons.Widgets.Add(BuildButton("Render", ()=>RenderClicked?.Invoke()));
        renderButtons.Widgets.Add(BuildButton("...", ()=>RenderSettingsClicked?.Invoke(), 80, 80));

        _stackPanel.Widgets.Add(frameLooperAndShaderButtonStack);
        _stackPanel.Widgets.Add(renderButtons);
        mainGrid.Widgets.Add(inspectorWidget);
        mainGrid.Widgets.Add(_stackPanel);

        return mainGrid;
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