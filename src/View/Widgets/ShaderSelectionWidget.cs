using System;
using System.IO;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.File;

namespace QMEditor.View;

public class ShaderSelectionWidget : Grid {

    public Action<string> ShaderSelected;
    public Action<float> ShaderUserVariableSelected;

    private Button _shaderSelectButton;

    public ShaderSelectionWidget() {
        BuildUI();
    }

    private void BuildUI() {
        DefaultRowProportion = new Proportion(ProportionType.Pixels, 60);

        // Shader stack
        var shaderStack = new HorizontalStackPanel() { VerticalAlignment = VerticalAlignment.Center, Spacing = 5 };

        shaderStack.Widgets.Add(new Label() {
            Text = "Shader:", Height=40
        });
        _shaderSelectButton = new Button() {
            Content = new Label() {
                Text = "None", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            Width = 100, Height = 40
        };

        _shaderSelectButton.Click += (s, e) => { SelectShader(); };
        shaderStack.Widgets.Add(_shaderSelectButton);
        
        // User Variable Stack
        var userVariableStack = new HorizontalStackPanel() { VerticalAlignment = VerticalAlignment.Center, Spacing = 5 };

        userVariableStack.Widgets.Add(new Label() {
            Text = "Effect Parameter", Height=40
        });
        var userVariableSelector = new FloatSelectorWidget(100, 40);

        userVariableSelector.ValueChanged += (v) => { ShaderUserVariableSelected?.Invoke(v); };
        userVariableStack.Widgets.Add(userVariableSelector);

        Widgets.Add(shaderStack);
        SetRow(userVariableStack, 1);
        Widgets.Add(userVariableStack);
    }

    private void SelectShader() {
        var fileDialog = new FileDialog(FileDialogMode.OpenFile) {
            Title = "Select shader file",
            Filter = "*.fx", FilePath = Path.GetFullPath("assets\\shaders\\default.fx")
        };
        fileDialog.Closed += (s, e) => {
            if (!fileDialog.Result) return;
            var shaderName = (Label)_shaderSelectButton.Content;
            shaderName.Text = Path.GetFileNameWithoutExtension(fileDialog.FilePath);
            ShaderSelected?.Invoke(fileDialog.FilePath);
        };
        fileDialog.ShowModal(Global.Desktop);
    }

}