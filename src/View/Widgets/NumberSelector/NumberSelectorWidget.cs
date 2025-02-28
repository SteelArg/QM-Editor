using System;
using System.Globalization;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using QMEditor.Controllers;

namespace QMEditor.View;

public abstract class NumberSelectorWidget : Panel {

    public Action<float> ValueChanged;

    private TextBox _textBox;

    protected float _value;
    protected float? _minValue;
    protected float? _maxValue;

    public NumberSelectorWidget(int? width = null, int? height = null, float? minValue = null, float? maxValue = null, float startValue = 0) {
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
        int buttonSize = Height.Value;
        int textBoxWidth = Width.Value-buttonSize*2;
        int textBoxHeight = Height.Value;

        var grid = new Grid {
            DefaultColumnProportion = new Proportion(ProportionType.Pixels, buttonSize), Width = Width.Value, Height = Height.Value
        };
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, textBoxWidth));

        _textBox = new TextBox {
            Text = _value.ToString(),
            Width = textBoxWidth, Height = textBoxHeight,
        };
        _textBox.TextChangedByUser += (s, e) => { OnTextChanged(); };

        grid.Widgets.Add(_textBox);
        grid.Widgets.Add(BuildButton("-", buttonSize, DecreaseValue, [1,0]));
        grid.Widgets.Add(BuildButton("+", buttonSize, IncreaseValue, [2,0]));

        Widgets.Add(grid);
    }

    private Button BuildButton(string text, int size, Action callback = null, int[] gridPos = null) {
        var button = new Button {
            Content = new Label {
                Text = text, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center, Font = FontLoader.GetFont(20, FontLoader.FontType.Bold),
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

    private void IncreaseValue() => ChangeValue(GetIncrement());
    private void DecreaseValue() => ChangeValue(GetIncrement() * -1f);
    private void ChangeValue(float delta) => SetValue(_value + delta);

    private float GetIncrement() {
        if (Input.KeyHeld(Keys.LeftAlt) && Input.KeyHeld(Keys.LeftShift))
            return 1000f;
        if (Input.KeyHeld(Keys.LeftAlt))
            return 100f;
        if (Input.KeyHeld(Keys.LeftShift))
            return 10f;
        if (Input.KeyHeld(Keys.LeftControl))
            return 0.1f;
        return 1f;
    }

    public void SetValue(float newValue) {
        newValue = Math.Max(newValue, (float)(_minValue ?? newValue));
        newValue = Math.Min(newValue, (float)(_maxValue ?? newValue));

        if (newValue == _value) return;

        _value = MathF.Round(newValue, 3);
        _textBox.Text = ValueToString(_value);
        ValueChanged?.Invoke(newValue);
    }

    private void OnTextChanged() {
        string text = _textBox.Text;
        string numbersOnlyText = GetNumbersOnlyText(text);

        numbersOnlyText = BackspaceEdgeCase(numbersOnlyText);

        float val = 0f;
        try {
            val = float.Parse(numbersOnlyText.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture);
        } catch {}
        SetValue(val);
        _textBox.Text = _value.ToString();
    }

    protected abstract string GetNumbersOnlyText(string text);
    
    protected abstract string BackspaceEdgeCase(string text);

    protected abstract string ValueToString(float value);

}