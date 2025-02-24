using System;
using Myra.Graphics2D.UI;
using QMEditor.Model;
using QMEditor.View;

namespace QMEditor.Controllers;

public class Inspector {

    public Action<EditCommandBase> ExecuteCommand;

    private Label _inspectableName;
    private Button[] _commandButtons;
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
        grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 35));
        grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 35));

        var labelGrid = new Myra.Graphics2D.UI.Grid { Height = 35, DefaultColumnProportion = Proportion.Auto, HorizontalAlignment = HorizontalAlignment.Right };
        _inspectableName = new Label {
            Text = "Not selected", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center, Font = FontLoader.GetFont(30, FontLoader.FontType.Bold),
            VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Width = 400
        };
        var closeInspectorButton = new Button {
            Content = new Label {
                Text = "x", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center, Font = FontLoader.GetFont(25, FontLoader.FontType.Bold),
                VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Center, 
            },
            Width = 30, Height = 30
        };
        closeInspectorButton.Click += (s, e) => { Inspect(null); };
        labelGrid.Widgets.Add(_inspectableName);
        Myra.Graphics2D.UI.Grid.SetColumn(closeInspectorButton, 1);
        labelGrid.Widgets.Add(closeInspectorButton);

        var commandButtonsGrid = new Myra.Graphics2D.UI.Grid { Height = 35, DefaultColumnProportion = Proportion.Auto, HorizontalAlignment = HorizontalAlignment.Center };
        _commandButtons = [
            BuildButton("Copy", () => {
                if (_inspection == null || _inspection.InspectedObject is not GridObject) return;
                var copyCommand = new CopyGridObjectCommand((GridObject)_inspection.InspectedObject);
                ExecuteCommand?.Invoke(copyCommand);
            }),
            BuildButton("Delete", () => {
                if (_inspection == null || _inspection.InspectedObject is not GridObject) return;
                var deleteCommand = new DeleteGridObjectCommand((GridObject)_inspection.InspectedObject);
                ExecuteCommand?.Invoke(deleteCommand);
            })
        ];
        commandButtonsGrid.Widgets.Add(_commandButtons[0]);
        Myra.Graphics2D.UI.Grid.SetColumn(_commandButtons[1], 1);
        commandButtonsGrid.Widgets.Add(_commandButtons[1]);
        Myra.Graphics2D.UI.Grid.SetRow(commandButtonsGrid, 1);

        _inspectableDataPanel = new VerticalStackPanel {
            Spacing = 5, ShowGridLines = false
        };
        Myra.Graphics2D.UI.Grid.SetRow(_inspectableDataPanel, 2);

        grid.Widgets.Add(labelGrid);
        grid.Widgets.Add(commandButtonsGrid);
        grid.Widgets.Add(_inspectableDataPanel);

        ClearProperties();

        return grid;
    }

    private void RebuildProperties() {
        _inspectableName.Text = _inspection.GetName();

        foreach (InspectionProperty property in _inspection.GetProperties()) {
            _inspectableDataPanel.Widgets.Add(BuildProperty(property));
        }

        foreach (Button commandButton in _commandButtons) {
            commandButton.Enabled = _inspection != null && _inspection.InspectedObject is GridObject;
        }
    }

    private void ClearProperties() {
        _inspectableName.Text = "Not Selected";
        _inspectableDataPanel.Widgets.Clear();
        foreach (Button commandButton in _commandButtons) {
            commandButton.Enabled = false;
        }
    }

    private Widget BuildProperty(InspectionProperty property) {
        Widget widget = _propertiesView.BuildPropertyWidget(property);

        widget.Enabled = property.IsEditable();

        return widget;
    }

    private Button BuildButton(string buttonLabel, Action clickHandler, int width = 100, int height = 30) {
        var button = new Button {
            Content = new Label {
                Text = buttonLabel, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center, Font = FontLoader.GetFont(25, FontLoader.FontType.Bold),
                VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center,
            },
            Width = width, Height = height
        };
        button.Click += (s, e) => { clickHandler.Invoke(); };
        return button;
    }

}
