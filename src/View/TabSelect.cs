using System;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace QMEditor.View;

public class TabSelect {

    public Action<int> TabSelected;

    private static readonly string[] buttonNames = {"Settings", "Scene", "Character", "Assets"};

    public Widget BuildUI() {
        var grid = new Grid();

        for (int i = 0; i < buttonNames.Length; i++) {
            grid.Widgets.Add(BuildButton(buttonNames[i], i));
        }

        return grid;
    }

    private Button BuildButton(string buttonText, int id) {
        var button = new Button();
        button.Content = new Label {Text = buttonText, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center
            };
        button.HorizontalAlignment = HorizontalAlignment.Center;
        button.VerticalAlignment = VerticalAlignment.Center;
        button.Width = 1200;
        button.Height = 1200;
        Grid.SetColumn(button, id);
        button.Click += (s, a) => {TabSelected?.Invoke(id);};
        return button;
    }

}