using Myra.Graphics2D.UI;

namespace QMEditor.View;

public class NewWorldBuilderWidget : Grid {

    private IntSelectorWidget[] _sizeSelectors;
    private CheckButton _fillCheckButton;

    public NewWorldBuilderWidget() : base() {
        BuildUI();
    }

    public int[] GetWorldSize() => [_sizeSelectors[0].Value, _sizeSelectors[1].Value];
    public bool GetFillGrid() => _fillCheckButton.IsChecked;

    private void BuildUI() {
        RowsProportions.Add(new Proportion(ProportionType.Pixels, 50));
        RowsProportions.Add(new Proportion(ProportionType.Pixels, 50));
        DefaultColumnProportion = new Proportion(ProportionType.Auto);

        var xSizeSelector = new IntSelectorWidget(80, 30, 1, null, 1) { VerticalAlignment = VerticalAlignment.Center };
        var ySizeSelector = new IntSelectorWidget(80, 30, 1, null, 1) { VerticalAlignment = VerticalAlignment.Center };
        _sizeSelectors = [xSizeSelector, ySizeSelector];
        var sizeSelectStack = new HorizontalStackPanel {
            Spacing = 8, ShowGridLines = false,
            VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left,
        };
        sizeSelectStack.Widgets.Add(new Label { Text="X", Width = 10 });
        sizeSelectStack.Widgets.Add(xSizeSelector);
        sizeSelectStack.Widgets.Add(new Label { Text="Y", Width = 10 });
        sizeSelectStack.Widgets.Add(ySizeSelector);

        _fillCheckButton = new CheckButton() {
            Content = new Label() {
                Text = "Fill the World Grid with a Tile", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center
            },
            VerticalAlignment = VerticalAlignment.Center,
            Height = 40,
            IsChecked = true
        };
        SetRow(_fillCheckButton, 1);

        Widgets.Add(sizeSelectStack);
        Widgets.Add(_fillCheckButton);
    }

}