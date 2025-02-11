using Myra.Graphics2D.UI;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor.Controllers;

public class Inspector {

    private Label _inspectableName;
    private VerticalStackPanel _inspectableDataPanel;
    private InspectionData _inspection;

    private readonly InspectorPropertiesView _propertiesView;

    public Inspector() {
        _propertiesView = new InspectorPropertiesView(this);
    }

    public void Inspect(IInspectable inspectable) {
        ClearProperties();
        if (inspectable == null) return;
        _inspection = inspectable.GetInspectionData();
        RebuildProperties();
    }

    public Widget BuildUI() {
        var grid = new Myra.Graphics2D.UI.Grid {
            VerticalAlignment = VerticalAlignment.Top,
            DefaultRowProportion = Proportion.Auto
        };
        grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 70));

        _inspectableName = new Label {
            Text = "Not selected", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center,
        };

        _inspectableDataPanel = new VerticalStackPanel {
            Spacing = 5, ShowGridLines = false
        };
        Myra.Graphics2D.UI.Grid.SetRow(_inspectableDataPanel, 1);

        grid.Widgets.Add(_inspectableName);
        grid.Widgets.Add(_inspectableDataPanel);

        return grid;
    }

    private void RebuildProperties() {
        _inspectableName.Text = _inspection.GetName();

        foreach (InspectionProperty property in _inspection.GetProperties()) {
            _inspectableDataPanel.Widgets.Add(BuildProperty(property));
        }
    }

    private void ClearProperties() {
        _inspectableName.Text = "NotSelected";
        _inspectableDataPanel.Widgets.Clear();
    }

    private Widget BuildProperty(InspectionProperty property) {
        Widget widget = _propertiesView.BuildPropertyWidget(property);

        widget.Enabled = property.IsEditable();

        return widget;
    }

    

}
