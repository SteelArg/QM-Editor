namespace QMEditor.View;

public class FloatSelectorWidget : NumberSelectorWidget {

    public float Value { get { return _value; } }

    public FloatSelectorWidget(int? width = null, int? height = null, float? minValue = null, float? maxValue = null, float startValue = 0f)
        : base(width, height, minValue, maxValue, startValue) {}

    public void SetValue(int value) {
        SetValue((float)value);
    }

    protected override string GetNumbersOnlyText(string text) {
        string numbersOnlyText = "0";

        foreach (char c in text) {
            if (char.IsDigit(c) || c == '.')
                numbersOnlyText += c;
        }
        return numbersOnlyText;
    }

    protected override string BackspaceEdgeCase(string text) => text;
    protected override string ValueToString(float value) => value.ToString();

}