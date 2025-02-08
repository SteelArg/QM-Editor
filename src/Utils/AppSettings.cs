using Microsoft.Xna.Framework;
using QMEditor.Model;

namespace QMEditor;

public class AppSettings : Singleton<AppSettings> {

    private const string SettingsPath = "assets\\settings\\settings.txt";

    public static Setting<float> Volume {get => Instance._volume;}
    public static Setting<string> Version {get => Instance._version;}
    public static Setting<TwoFloats> RenderOffset {get => Instance._renderOffset;}
    public static Setting<TwoFloats> RenderTileTopSize {get => Instance._renderTileTopSize;}
    public static Setting<int> RenderTileHeight {get => Instance._renderTileHeight;}
    public static Setting<int> RenderFrameCount {get => Instance._renderFrameCount;}
    public static Setting<int> RenderFrameDuration {get => Instance._renderFrameDuration;}
    public static Setting<TwoFloats> RenderOutputSize {get => Instance._renderOutputSize;}
    public static Setting<int> RenderOutputUpscaling {get => Instance._renderOutputUpscaling;}

    private Setting<float> _volume;
    private Setting<string> _version;
    private Setting<TwoFloats> _renderOffset;
    private Setting<TwoFloats> _renderTileTopSize;
    private Setting<int> _renderTileHeight;
    private Setting<int> _renderFrameCount;
    private Setting<int> _renderFrameDuration;
    private Setting<TwoFloats> _renderOutputSize;
    private Setting<int> _renderOutputUpscaling;
    private StringDataParser _dataParser;

    public AppSettings() : base() {
        _volume = new Setting<float>("volume", 1f);
        _version = new Setting<string>("editor_version", "v0");
        _renderOffset = new Setting<TwoFloats>("render_offset", TwoFloats.Zero);
        _renderTileTopSize = new Setting<TwoFloats>("render_tile_top_size", TwoFloats.Zero);
        _renderTileHeight = new Setting<int>("render_tile_height", 0);
        _renderFrameCount = new Setting<int>("render_frame_count", 0);
        _renderFrameDuration = new Setting<int>("render_frame_duration", 0);
        _renderOutputSize = new Setting<TwoFloats>("render_output_size", TwoFloats.Zero);
        _renderOutputUpscaling = new Setting<int>("render_output_upscaling", 0);
        _dataParser = new StringDataParser(SettingsPath);
        Load();
    }

    public void Save() {
        _volume.SaveTo(_dataParser);
        _version.SaveTo(_dataParser);
        _renderOffset.SaveTo(_dataParser);
        _renderTileTopSize.SaveTo(_dataParser);
        _renderTileHeight.SaveTo(_dataParser);
        _renderFrameCount.SaveTo(_dataParser);
        _renderFrameDuration.SaveTo(_dataParser);
        _renderOutputSize.SaveTo(_dataParser);
        _renderOutputUpscaling.SaveTo(_dataParser);
        _dataParser.Save();
    }

    public void Load() {
        _dataParser.Load();
        _dataParser.Ensure("volume", 1f.ToString());

        _volume.SetValue(float.Parse(_dataParser.GetValue(_volume.SettingId)));
        _version.SetValue(_dataParser.GetValue(_version.SettingId));
        _renderOffset.SetValue(TwoFloats.FromString(_dataParser.GetValue(_renderOffset.SettingId)));
        _renderTileTopSize.SetValue(TwoFloats.FromString(_dataParser.GetValue(_renderTileTopSize.SettingId)));
        _renderTileHeight.SetValue(int.Parse(_dataParser.GetValue(_renderTileHeight.SettingId)));
        _renderFrameCount.SetValue(int.Parse(_dataParser.GetValue(_renderFrameCount.SettingId)));
        _renderFrameDuration.SetValue(int.Parse(_dataParser.GetValue(_renderFrameDuration.SettingId)));
        _renderOutputSize.SetValue(TwoFloats.FromString(_dataParser.GetValue(_renderOutputSize.SettingId)));
        _renderOutputUpscaling.SetValue(int.Parse(_dataParser.GetValue(_renderOutputUpscaling.SettingId)));
    }

}

public class Setting<T> {

    public T Value {get => _value;}
    public string SettingId {get => _settingId;}

    private string _settingId;
    private T _value;

    public Setting(string settingId, T value) {
        _settingId = settingId;
        _value = value;
    }

    public void SaveTo(StringDataParser dataParser) {
        dataParser.SetValue(_settingId, _value.ToString());
    }

    public void SetValue(T value) {
        _value = value;
    }

    public override string ToString() {
        return _value.ToString();
    }

}

public struct TwoFloats {

    public readonly float First;
    public readonly float Second;

    public static readonly TwoFloats Zero = new TwoFloats(0f, 0f);

    public TwoFloats(float first, float second) {
        First = first;
        Second = second;
    }

    public override string ToString() {
        return $"{First} {Second}";
    }

    public static TwoFloats FromString(string str) {
        string[] numbers = str.Split(" ");
        return new TwoFloats(float.Parse(numbers[0]), float.Parse(numbers[1]));
    }

    public Vector2 ToVector2() => new Vector2(First, Second);
    public int[] ToIntArray() => [(int)First, (int)Second];

}