using System;
using Myra.Graphics2D.UI;

namespace QMEditor.View;

public class FrameLooperWidget : Panel {

    public Action TogglePauseClicked;
    public Action NextFrameClicked;
    public Action PrevFrameClicked;

    private Grid _grid;
    private Label _frameCount;
    private Button _pauseButton;

    public FrameLooperWidget() {
        BuildUI();
    }

    public void SetCurrentFrame(int currentFrame) {
        _frameCount.Text = $"Frame: {currentFrame+1}";
    }

    private void BuildUI() {
        _grid = new Grid{
            DefaultColumnProportion = new Proportion(ProportionType.Auto),
            DefaultRowProportion = new Proportion(ProportionType.Auto)
        };

        _frameCount = new Label {
            Text = "Frame: 0", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center,
            Height = 50
        };
        Grid.SetColumnSpan(_frameCount, 3);
        _grid.Widgets.Add(_frameCount);

        _pauseButton = BuildButton("Pause", () => {
            TogglePauseClicked?.Invoke(); Label label = (Label)_pauseButton.Content;
            label.Text = label.Text == "Pause" ? "Play" : "Pause";
        }, 1);
        _grid.Widgets.Add(_pauseButton);
        _grid.Widgets.Add(BuildButton("Prev", () => PrevFrameClicked?.Invoke(), 1, 1));
        _grid.Widgets.Add(BuildButton("Next", () => NextFrameClicked?.Invoke(), 1, 2));

        Widgets.Add(_grid);
    }

    private Button BuildButton(string label, Action clickEvent, int row = 0, int column = 0) {
        var button = new Button {
            Content = new Label {
                Text = label, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            Width = 70, Height = 70, Padding = new Myra.Graphics2D.Thickness(20),
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
        };
        button.Click += (s, e) => {clickEvent?.Invoke();};

        if (row > 0)
            Grid.SetRow(button, row);
        if (column > 0)
            Grid.SetColumn(button, column);

        return button;
    }

}