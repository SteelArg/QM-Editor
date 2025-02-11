using Myra.Graphics2D.UI;
using QMEditor.Controllers;
using QMEditor.Model;

namespace QMEditor.View;

public class InspectorPropertiesView {

    private Inspector _inspector;

    public InspectorPropertiesView(Inspector inspector) {
        _inspector = inspector;
    }

    public Widget BuildPropertyWidget(InspectionProperty property) {
        StackPanel stack = null;

        switch (property.Type) {
            case InspectionProperty.PropertyType.Integer:
                AddInteger(ref stack, property);
                break;
            case InspectionProperty.PropertyType.String:
                AddString(ref stack, property);
                break;
            case InspectionProperty.PropertyType.Check:
                AddBool(ref stack, property);
                break;
            case InspectionProperty.PropertyType.GridPosition:
                AddGridPosition(ref stack, property);
                break;
            case InspectionProperty.PropertyType.InspectableLink:
                AddSingleInspectableLink(ref stack, property);
                break;
            case InspectionProperty.PropertyType.InspectablesLinkArray:
                AddInspectablesLinkArray(ref stack, property);
                break;
        }

        return stack;
    }

    private StackPanel BuildDefaultPropertyStack(string stackName) {
        var stack = new HorizontalStackPanel {
            Spacing = 5, ShowGridLines = false, Height = 50
        };

        var label = new Label {
            Text = stackName, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Left
        };

        stack.Widgets.Add(label);
        return stack;
    }

    private StackPanel BuildVerticalArrayPropertyStack(string stackName) {
        var stack = new VerticalStackPanel {
            Spacing = 0, ShowGridLines = false
        };

        var label = new Label {
            Text = stackName, TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center
        };

        stack.Widgets.Add(label);
        return stack;
    }

    private void AddInteger(ref StackPanel stack, InspectionProperty property) {
        stack = BuildDefaultPropertyStack(property.GetName());
        
        var intSelector = new IntSelectorWidget(80, 40, null, null, (int)property.GetValue());
        intSelector.ValueChanged += (v) => { property.SetValue(v); };
        stack.Widgets.Add(intSelector);
    }

    private void AddString(ref StackPanel stack, InspectionProperty property) {
        stack = BuildDefaultPropertyStack(property.GetName());
        
        var textBox = new TextBox {
            Text = (string)property.GetValue(),
            HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center,
            Width = 120, Height = 40
        };
        textBox.TextChangedByUser += (s, e) => { property.SetValue(textBox.Text); };
        stack.Widgets.Add(textBox);
    }

    private void AddBool(ref StackPanel stack, InspectionProperty property) {
        stack = BuildDefaultPropertyStack(property.GetName());
        
        var checkButton = new CheckButton {
            HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center,
            Width = 40, Height = 40
        };
        checkButton.Click += (s, e) => { property.SetValue(checkButton.IsChecked); };
        stack.Widgets.Add(checkButton);
    }

    private void AddGridPosition(ref StackPanel stack, InspectionProperty property) {
        stack = BuildDefaultPropertyStack(property.GetName());

        int[] defaultPos = (int[])property.GetValue() ?? [0,0];
        var xSelector = new IntSelectorWidget(60, 40, 0, null, defaultPos[0]);
        var ySelector = new IntSelectorWidget(60, 40, 0, null, defaultPos[1]);
        xSelector.ValueChanged += (v) => { property.SetValue(new int[] {xSelector.Value, ySelector.Value}); };
        ySelector.ValueChanged += (v) => { property.SetValue(new int[] {xSelector.Value, ySelector.Value}); };

        stack.Widgets.Add(new Label { Text="X" });
        stack.Widgets.Add(xSelector);
        stack.Widgets.Add(new Label { Text="Y" });
        stack.Widgets.Add(ySelector);
    }

    private void AddSingleInspectableLink(ref StackPanel stack, InspectionProperty property) {
        stack = BuildDefaultPropertyStack(property.GetName());
        AddInspectableButton(ref stack, (IInspectable)property.GetValue());
    }

    private void AddInspectablesLinkArray(ref StackPanel stack, InspectionProperty property) {
        stack = BuildVerticalArrayPropertyStack(property.GetName());

        var inspectables = (IInspectable[])property.GetValue();
        
        foreach (IInspectable inspectable in inspectables) {
            StackPanel inspectableStack = new HorizontalStackPanel { Spacing = 5, ShowGridLines = false };
            AddInspectableButton(ref inspectableStack, inspectable);
            stack.Widgets.Add(inspectableStack);
        }
    
    }

    private void AddInspectableButton(ref StackPanel stack, IInspectable inspectable) {
        InspectionData inspectionData = inspectable.GetInspectionData();
        
        var linkButton = new Button {
            Content = new Label { Text=inspectionData.GetName(), TextAlign=FontStashSharp.RichText.TextHorizontalAlignment.Center, VerticalAlignment=VerticalAlignment.Center },
            VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left,
            Height = 40
        };

        linkButton.Click += (s, e) => { _inspector.Inspect(inspectable); };

        stack.Widgets.Add(linkButton);
    }

}