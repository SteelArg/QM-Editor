using System;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace QMEditor.View;

public class CharacterEditorView {

    public Action CharacterCreated;
    public Action<int> AccessoryRemoved;

    private Grid _mainGrid;
    private Button _createButton;
    private VerticalStackPanel _accessoryStack;

    public CharacterEditorView() {}

    public Widget BuildUI() {
        _mainGrid = new Grid();
        Grid.SetColumnSpan(_mainGrid, 2);
        _mainGrid.RowsProportions.Add(new Proportion(ProportionType.Fill));
        _mainGrid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 200));
        _mainGrid.ColumnsProportions.Add(new Proportion(ProportionType.Fill));
        _mainGrid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, 500));

        // Create button
        _createButton = new Button() {
            Content = new Label() {
                Text = "Create", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom,
            Width = 200, Height = 120,
            Margin = new Thickness(20)
        };
        _createButton.Click += (s, a) => { CharacterCreated?.Invoke(); };
        Grid.SetColumnSpan(_createButton, 2);
        Grid.SetRow(_createButton, 2);
        _mainGrid.Widgets.Add(_createButton);

        // Accessory list
        _accessoryStack = new VerticalStackPanel() {
            Spacing = 10, ShowGridLines = true
        };
        Grid.SetColumn(_accessoryStack, 2);
        _mainGrid.Widgets.Add(_accessoryStack);

        return _mainGrid;
    }

    public void AddAccessory(string name) {
        var panel = new Panel();

        var removeButton = new Button() {
            Content = new Label() {
                Text = "Remove", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center
            },
            HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center
        };
        removeButton.Click += (s, a) => {
            Widget w = (Widget)s;
            VerticalStackPanel stack = (VerticalStackPanel)w.Parent.Parent;
            AccessoryRemoved?.Invoke(stack.Widgets.IndexOf(w.Parent));
        };

        var nameLabel = new Label() {
            Text = name, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center
        };
        
        panel.Widgets.Add(nameLabel);
        panel.Widgets.Add(removeButton);
        _accessoryStack.Widgets.Add(panel);
    }

    public void RemoveAccessory(int accessoryId) => _accessoryStack.Widgets.RemoveAt(accessoryId);

}