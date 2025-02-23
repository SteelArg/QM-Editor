using System;
using System.IO;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.File;
using QMEditor.Controllers;

namespace QMEditor.View;

public class ShaderSelectionWidget : Grid {

    public Action<string> ShaderSelected;
    public Action<float> ShaderUserVariableSelected;

    private Button _shaderSelectButton;
    private VerticalStackPanel _userVariableStack;
    private FloatSelectorWidget _userVariableSelectorNumber;
    private HorizontalSlider _userVariableSelectorSlider;

    public ShaderSelectionWidget() {
        BuildUI();
    }

    private void BuildUI() {
        DefaultRowProportion = new Proportion(ProportionType.Auto);

        // Shader stack
        var shaderStack = new HorizontalStackPanel() { VerticalAlignment = VerticalAlignment.Center, Spacing = 5 };

        shaderStack.Widgets.Add(new Label() {
            Text = "Shader:", Font = FontLoader.GetFont(20), Height=30
        });
        _shaderSelectButton = new Button() {
            Content = new Label() {
                Text = "None", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center, Font = FontLoader.GetFont(20),
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            Width = 200, Height = 30
        };

        _shaderSelectButton.Click += (s, e) => { SelectShader(); };
        shaderStack.Widgets.Add(_shaderSelectButton);
        
        // User Variable Stack
        _userVariableStack = new VerticalStackPanel() { VerticalAlignment = VerticalAlignment.Center, Spacing = 5 };
        _userVariableStack.Widgets.Add(new Label() {
            Text = "Effect Parameter", Font = FontLoader.GetFont(20), Height=30
        });

        _userVariableSelectorNumber = new FloatSelectorWidget(120, 30);
        _userVariableSelectorNumber.ValueChanged += (v) => { ShaderUserVariableSelected?.Invoke(v); };
        _userVariableStack.Widgets.Add(_userVariableSelectorNumber);

        _userVariableSelectorSlider = new HorizontalSlider {
            Minimum = 0f, Maximum = 1f,
            Width = 250, Height = 25
        };
        _userVariableSelectorSlider.ValueChangedByUser += (s, e) => { ShaderUserVariableSelected?.Invoke(_userVariableSelectorSlider.Value); };

        Widgets.Add(shaderStack);
        SetRow(_userVariableStack, 1);
        Widgets.Add(_userVariableStack);
    }

    private void SelectShader() {
        var fileDialog = new FileDialog(FileDialogMode.OpenFile) {
            Title = "Select shader file",
            Filter = "*.fx", FilePath = Path.GetFullPath("assets\\shaders\\default.fx")
        };
        fileDialog.Closed += (s, e) => {
            if (!fileDialog.Result) return;
            ShaderSelected?.Invoke(fileDialog.FilePath);
        };
        fileDialog.ShowModal(Global.Desktop);
    }

    public void SetEffectNameByPath(string effectPath, float[] userVariableConstraints) {
        var shaderName = (Label)_shaderSelectButton.Content;
        shaderName.Text = Path.GetFileNameWithoutExtension(effectPath) ?? "None";

        _userVariableStack.Widgets.RemoveAt(1);
        if (userVariableConstraints == null) {
            _userVariableStack.Widgets.Add(_userVariableSelectorNumber);
        }
        else {
            _userVariableStack.Widgets.Add(_userVariableSelectorSlider);
            _userVariableSelectorSlider.Minimum = userVariableConstraints[0];
            _userVariableSelectorSlider.Maximum = userVariableConstraints[1];
        }
    }

    public void SetEffectUserVariable(float userVariable) {
        _userVariableSelectorNumber.SetValue(userVariable);
        _userVariableSelectorSlider.Value = userVariable;
    }

}