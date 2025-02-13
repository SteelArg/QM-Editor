using System;
using Myra.Graphics2D.UI;
using QMEditor.Controllers;
using QMEditor.Model;

namespace QMEditor.View;

public class RenderSettingsWidget : Myra.Graphics2D.UI.Grid {

    private IntSelectorWidget[] _renderOffset;
    private IntSelectorWidget[] _outputSize;
    private IntSelectorWidget _frameDuration;
    private IntSelectorWidget _frameCount;
    private IntSelectorWidget _outputUpscaling;

    public RenderSettingsWidget() {
        BuildUI();
    }

    public int[] GetRenderOffset() => [_renderOffset[0].Value, _renderOffset[1].Value];
    public int[] GetOutputSize() => [_outputSize[0].Value, _outputSize[1].Value];
    public int GetFrameDuration() => _frameDuration.Value;
    public int GetFrameCount() => _frameCount.Value;
    public int GetOutputUpscaling() => _outputUpscaling.Value;

    public void WriteToAppSettings(bool save = true) {
        AppSettings.RenderOffset.Set(GetRenderOffset());
        AppSettings.RenderFrameDuration.Set(GetFrameDuration());
        AppSettings.RenderFrameCount.Set(GetFrameCount());
        AppSettings.RenderOutputSize.Set(GetOutputSize());
        AppSettings.RenderOutputUpscaling.Set(GetOutputUpscaling());
        
        if (save)
            AppSettings.Instance.Save();
    }

    private void BuildUI() {
        DefaultColumnProportion = Proportion.Auto;
        DefaultRowProportion = new Proportion(ProportionType.Pixels, 50);

        // Render Offset
        (Widget renderOffsetWidget, _renderOffset) = BuildIntArraySelector(AppSettings.RenderOffset.Get());
        Widgets.Add(renderOffsetWidget);

        // Frame Count
        (Widget frameCountWidget, _frameCount) = BuildSingleIntSelector(AppSettings.RenderFrameCount.Get(), "Frame Count", 1, 1);
        Widgets.Add(frameCountWidget);

        // Frame Duration
        (Widget frameDurationWidget, _frameDuration) = BuildSingleIntSelector(AppSettings.RenderFrameDuration.Get(), "Frame Duration", 2, 0);
        Widgets.Add(frameDurationWidget);

        // Render Size
        (Widget renderSizeWidget, _outputSize) = BuildIntArraySelector(AppSettings.RenderOutputSize.Get(), 3, 1);
        Widgets.Add(renderSizeWidget);

        // Render Upscaling
        (Widget renderUpscalingWidget, _outputUpscaling) = BuildSingleIntSelector(AppSettings.RenderOutputUpscaling.Get(), "Output Upscaling", 4, 1);
        Widgets.Add(renderUpscalingWidget);

        // Center button
        var centerButton = new Button {
            Content = new Label {
                Text="Center", TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            },
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Width = 80, Height = 30
        };
        centerButton.Click += (s, e) => { CenterWorldRender(); };
        SetRow(centerButton, 5);
        Widgets.Add(centerButton);
    }

    private (Widget, IntSelectorWidget[]) BuildIntArraySelector(int[] defaultValue, int row = 0, int? minValue = null) {
        var xSelector = new IntSelectorWidget(100, 40, minValue, null, defaultValue[0]);
        var ySelector = new IntSelectorWidget(100, 40, minValue, null, defaultValue[1]);        
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

    private void CenterWorldRender() {
        int[] worldSize = WorldRenderer.RenderSettings.CalculateGridSizeInPixels(World.Instance.Grid.Size);

        int upscaling = GetOutputUpscaling();
        int[] scaledWorldSize = [worldSize[0] * upscaling, worldSize[1] * upscaling];

        int[] outputSize = GetOutputSize();

        int doubleUpscaling = upscaling * 2;

        int[] offset = [(outputSize[0] - scaledWorldSize[0])/doubleUpscaling, (outputSize[1] - scaledWorldSize[1])/doubleUpscaling];
        offset[0] += (int)((World.Instance.Grid.Size[1]-1) * WorldRenderer.RenderSettings.StepX.X);

        _renderOffset[0].SetValue(offset[0]);
        _renderOffset[1].SetValue(offset[1]);
    }

}