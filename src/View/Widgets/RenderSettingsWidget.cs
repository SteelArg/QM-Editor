using Myra.Graphics2D.UI;

namespace QMEditor.View;

public class RenderSettingsWidget : Grid {

    private IntSelectorWidget[] _renderOffset;
    private IntSelectorWidget[] _outputSize;
    private IntSelectorWidget _frameDuration;
    private IntSelectorWidget _frameCount;
    private IntSelectorWidget _outputUpscaling;

    public RenderSettingsWidget() {
        BuildUI();
    }

    public TwoFloats GetRenderOffset() => new TwoFloats(_renderOffset[0].Value, _renderOffset[1].Value);
    public TwoFloats GetOutputSize() => new TwoFloats(_outputSize[0].Value, _outputSize[1].Value);
    public int GetFrameDuration() => _frameDuration.Value;
    public int GetFrameCount() => _frameCount.Value;
    public int GetOutputUpscaling() => _outputUpscaling.Value;

    private void BuildUI() {
        DefaultColumnProportion = Proportion.Auto;
        DefaultRowProportion = new Proportion(ProportionType.Pixels, 50);

        // Render Offset
        (Widget renderOffsetWidget, _renderOffset) = BuildTwoFloatSelector(AppSettings.RenderOffset.Value);
        Widgets.Add(renderOffsetWidget);

        // Frame Count
        (Widget frameCountWidget, _frameCount) = BuildSingleIntSelector(AppSettings.RenderFrameCount.Value, "Frame Count", 1, 1);
        Widgets.Add(frameCountWidget);

        // Frame Duration
        (Widget frameDurationWidget, _frameDuration) = BuildSingleIntSelector(AppSettings.RenderFrameDuration.Value, "Frame Duration", 2, 0);
        Widgets.Add(frameDurationWidget);

        // Render Size
        (Widget renderSizeWidget, _outputSize) = BuildTwoFloatSelector(AppSettings.RenderOutputSize.Value, 3, 1);
        Widgets.Add(renderSizeWidget);

        // Render Upscaling
        (Widget renderUpscalingWidget, _outputUpscaling) = BuildSingleIntSelector(AppSettings.RenderOutputUpscaling.Value, "Output Upscaling", 4, 1);
        Widgets.Add(renderUpscalingWidget);
    }

    private (Widget, IntSelectorWidget[]) BuildTwoFloatSelector(TwoFloats defaultValue, int row = 0, int? minValue = null) {
        var xSelector = new IntSelectorWidget(100, 40, minValue, null, (int)defaultValue.First);
        var ySelector = new IntSelectorWidget(100, 40, minValue, null, (int)defaultValue.Second);        
        var stack = new HorizontalStackPanel {
            Spacing = 5, ShowGridLines = false,
            HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center
        };
        stack.Widgets.Add(new Label { Text="X", Width = 10 });
        stack.Widgets.Add(xSelector);
        stack.Widgets.Add(new Label { Text="Y", Width = 10 });
        stack.Widgets.Add(ySelector);
        SetRow(stack, row);
        return (stack, [xSelector, ySelector]);
    }

    private (Widget, IntSelectorWidget) BuildSingleIntSelector(int defaultValue, string label, int row = 0, int? minValue = null) {
        var selector = new IntSelectorWidget(60, 40, minValue, null, defaultValue);
        var stack = new HorizontalStackPanel {
            Spacing = 5, ShowGridLines = false,
            HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center
        };
        stack.Widgets.Add(new Label { Text=$"{label}: " });
        stack.Widgets.Add(selector);
        SetRow(stack, row);
        return (stack, selector);
    }

}