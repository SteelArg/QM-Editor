using System;
using Myra.Graphics2D.UI;

namespace QMEditor.View;

public class WorldIOManagerView {

    public Action ClickedLoad;
    public Action ClickedSave;
    public Action ClickedNew;

    private Grid _grid;
    private Label _saveName;

    public WorldIOManagerView() {}

    public Widget BuildUI() {
        // Create grid
        _grid = new Grid() {
            DefaultColumnProportion = new Proportion(ProportionType.Auto),
            DefaultRowProportion = new Proportion(ProportionType.Auto),
            ColumnSpacing = 20, RowSpacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetColumnSpan(_grid, 2);
        
        // Save/Load/New buttons
        Button loadButton = BuildButton("Load");
        loadButton.Click += (s, a) => { ClickedLoad?.Invoke(); };

        Button saveButton = BuildButton("Save", column: 1);
        saveButton.Click += (s, a) => { ClickedSave?.Invoke(); };

        Button newButton = BuildButton("New", column: 2);
        newButton.Click += (s, a) => { ClickedNew?.Invoke(); };

        // Save name
        _saveName = new Label() {
            Text = string.Empty,
            TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        Grid.SetColumnSpan(_saveName, 3);
        Grid.SetRow(_saveName, 1);

        // Add to grid
        _grid.Widgets.Add(loadButton);
        _grid.Widgets.Add(saveButton);
        _grid.Widgets.Add(newButton);
        _grid.Widgets.Add(_saveName);
        
        return _grid;
    }

    public void SetSaveName(string save) => _saveName.Text = save;

    private Button BuildButton(string text, int width = 120, int height = 70, int column = 0) {
        var button = new Button() {
            Content = new Label () {
                Text = text, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            Width = width, Height = height
        };
        Grid.SetColumn(button, column);
        return button;
    }

}