using System;
using Myra.Graphics2D.UI;
using QMEditor.Controllers;

namespace QMEditor.View;

public class TabSelect {

    public Action<int> TabSelected;

    private static readonly string[] buttonNames = {"Settings", "Scene", "Character", "Assets"};

    public Widget BuildUI() {
        var grid = new Myra.Graphics2D.UI.Grid();

        for (int i = 0; i < buttonNames.Length; i++) {
            grid.Widgets.Add(BuildButton(buttonNames[i], i));
        }

        return grid;
    }

    private Button BuildButton(string buttonText, int id) {
        var button = new Button();
        button.Content = new Label {
            Text = buttonText, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center, Font = FontLoader.GetFont(40, FontLoader.FontType.Bold),
            VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center
        };
        button.HorizontalAlignment = HorizontalAlignment.Center;
        button.VerticalAlignment = VerticalAlignment.Center;
        button.Width = 1200;
        button.Height = 1200;
        Myra.Graphics2D.UI.Grid.SetColumn(button, id);
        button.Click += (s, a) => {TabSelected?.Invoke(id);};
        return button;
    }

}