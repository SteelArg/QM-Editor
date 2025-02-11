using System;
using Myra.Graphics2D.UI;

namespace QMEditor.View;

public class IntSelectorWidget : Panel {

    public int Value { get { return _value; } }
    public Action<int> ValueChanged;

    private TextBox _textBox;

    private int _value;
    private int? _minValue;
    private int? _maxValue;

    public IntSelectorWidget(int? width = null, int? height = null, int? minValue = null, int? maxValue = null, int startValue = 0) {
        if (maxValue.HasValue && minValue.HasValue && minValue > maxValue)
            throw new ArgumentException("maxValue cannot be smaller than minValue");

        Width = width ?? 100;
        Height = height ?? 40;

        _minValue = minValue;
        _maxValue = maxValue;
        _value = startValue;

        BuildUI();
    }

    private void BuildUI() {
        int buttonSize = Height.Value/2;
        int textBoxWidth = Width.Value-buttonSize;
        int textBoxHeight = Height.Value;

        var grid = new Grid {
            DefaultRowProportion = new Proportion(ProportionType.Pixels, buttonSize),
            Width = Width.Value, Height = Height.Value
        };
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, Width.Value - Height.Value/2));
        grid.ColumnsProportions.Add(Proportion.Fill);

        _textBox = new TextBox {
            Text = _value.ToString(),
            Width = textBoxWidth, Height = textBoxHeight,
        };
        _textBox.TextChangedByUser += (s, e) => { OnTextChanged(); };
        Grid.SetRowSpan(_textBox, 2);

        grid.Widgets.Add(_textBox);
        grid.Widgets.Add(BuildButton("+", buttonSize, IncreaseValue, [1,0]));
        grid.Widgets.Add(BuildButton("-", buttonSize, DecreaseValue, [1,1]));

        Widgets.Add(grid);
    }

    private Button BuildButton(string text, int size, Action callback = null, int[] gridPos = null) {
        var button = new Button {
            Content = new Label {
                Text = text, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center,
            Width = size, Height = size
        };

        button.Click += (s, e) => { callback?.Invoke(); };

        if (gridPos != null) {
            Grid.SetColumn(button, gridPos[0]);
            Grid.SetRow(button, gridPos[1]);
        }

        return button;
    }

    private void IncreaseValue() => ChangeValue(1);
    private void DecreaseValue() => ChangeValue(-1);
    private void ChangeValue(int delta) => SetValue(_value + delta);

    private void SetValue(int newValue) {
        int min = _minValue??newValue;
        int max = _maxValue??newValue;
        if (max < min) {
            if (_maxValue.HasValue)
                min = max-1;
            else
                max = min+1;
        }

        newValue = Math.Clamp(newValue, min, max);
        if (newValue == _value) return;

        _value = newValue;
        _textBox.Text = _value.ToString();
        ValueChanged?.Invoke(newValue);
    }

    private void OnTextChanged() {
        string text = _textBox.Text;
        string numbersOnlyText = "0";

        foreach (char c in text) {
            if (char.IsDigit(c))
                numbersOnlyText += c;
        }

        if (_minValue.HasValue && _minValue < 10 && Value == _minValue && numbersOnlyText.Length > 1 && numbersOnlyText[numbersOnlyText.Length-1] == _minValue.ToString()[0])
            numbersOnlyText = numbersOnlyText.Remove(numbersOnlyText.Length-1);

        SetValue(int.Parse(numbersOnlyText));
        _textBox.Text = Value.ToString();
    }

}