namespace QMEditor.View;

public class IntSelectorWidget : NumberSelectorWidget {

    public int Value { get { return (int)_value; } }

    public IntSelectorWidget(int? width = null, int? height = null, int? minValue = null, int? maxValue = null, int startValue = 0)
        : base(width, height, minValue, maxValue, startValue) {}

    public void SetValue(int value) {
        SetValue((float)value);
    }

    protected override string GetNumbersOnlyText(string text) {
        string numbersOnlyText = "0";

        foreach (char c in text) {
            if (char.IsDigit(c))
                numbersOnlyText += c;
        }
        return numbersOnlyText;
    }

    protected override string BackspaceEdgeCase(string text) {
        if (_minValue.HasValue && _minValue < 10 && _value == _minValue && text.Length > 1 && text[text.Length-1] == _minValue.ToString()[0])
            return text.Remove(text.Length-1);
        return text;
    }

    protected override string ValueToString(float value) {
        return ((int)value).ToString();
    }

}