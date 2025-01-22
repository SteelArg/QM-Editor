using System;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.File;

namespace QMEditor.View;

public class WorldIOManagerView {

    public Action ClickedLoad;
    public Action ClickedSave;

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
        
        // Save/Load buttons
        Button loadButton = BuildButton("Load");
        loadButton.Click += (s, a) => { ClickedLoad?.Invoke(); };

        Button saveButton = BuildButton("Save");
        saveButton.Click += (s, a) => { ClickedSave?.Invoke(); };
        Grid.SetColumn(saveButton, 1);

        // Save name
        _saveName = new Label() {
            Text = string.Empty,
            TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        Grid.SetColumnSpan(_saveName, 2);
        Grid.SetRow(_saveName, 1);

        // Add to grid
        _grid.Widgets.Add(loadButton);
        _grid.Widgets.Add(saveButton);
        _grid.Widgets.Add(_saveName);
        
        return _grid;
    }

    public void SetSaveName(string save) => _saveName.Text = save;

    private Button BuildButton(string text, int width = 120, int height = 70) {
        var button = new Button() {
            Content = new Label () {
                Text = text, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            Width = width, Height = height
        };
        return button;
    }

}