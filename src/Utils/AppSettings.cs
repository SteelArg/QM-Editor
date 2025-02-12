using System.Collections.Generic;
using Microsoft.Xna.Framework;
using QMEditor.Model;

namespace QMEditor;

public class AppSettings : Singleton<AppSettings> {

    private const string SettingsPath = "assets\\settings\\settings.txt";

    public static StringSetting Version {get => (StringSetting)Instance._settings["editor_version"]; }
    public static IntArraySetting RenderOffset {get => (IntArraySetting)Instance._settings["render_offset"]; }
    public static IntArraySetting RenderTileTopSize {get => (IntArraySetting)Instance._settings["render_tile_top_size"]; }
    public static IntSetting RenderTileHeight {get => (IntSetting)Instance._settings["render_tile_height"]; }
    public static IntSetting RenderFrameCount {get => (IntSetting)Instance._settings["render_frame_count"]; }
    public static IntSetting RenderFrameDuration {get => (IntSetting)Instance._settings["render_frame_duration"]; }
    public static IntArraySetting RenderOutputSize {get => (IntArraySetting)Instance._settings["render_output_size"]; }
    public static IntSetting RenderOutputUpscaling {get => (IntSetting)Instance._settings["render_output_upscaling"]; }

    private Dictionary<string, Setting> _settings;
    private StringDataParser _dataParser;

    public AppSettings() : base() {
        _settings = new Dictionary<string, Setting>();

        AddSettings([
            new StringSetting("editor_version"),
            new IntArraySetting("render_offset"),
            new IntArraySetting("render_tile_top_size"),
            new IntSetting("render_tile_height"),
            new IntSetting("render_frame_count"),
            new IntSetting("render_frame_duration"),
            new IntArraySetting("render_output_size"),
            new IntSetting("render_output_upscaling")
        ]);
        
        _dataParser = new StringDataParser(SettingsPath);
        Load();
    }

    public void Save() {
        foreach(Setting setting in _settings.Values) {
            setting.SaveTo(_dataParser);
        }
        _dataParser.Save();
    }

    public void Load() {
        _dataParser.Load();
        foreach (Setting setting in _settings.Values) {
            setting.LoadFrom(_dataParser);
        }
    }

    private void AddSettings(Setting[] settings) {
        foreach (Setting setting in settings) {
            AddSetting(setting);
        }
    }

    private void AddSetting(Setting setting) {
        _settings.Add(setting.SettingId, setting);
    }

}

public abstract class Setting {

    public string SettingId {get => _settingId;}

    private string _settingId;

    public Setting(string settingId) {
        _settingId = settingId;
    }

    public void SaveTo(StringDataParser dataParser) {
        dataParser.SetValue(_settingId, GetStringValue());
    }

    public void LoadFrom(StringDataParser dataParser){
        SetStringValue(dataParser.GetValue(_settingId));
    }

    public abstract void SetStringValue(string str);
    protected abstract string GetStringValue();

}

public class StringSetting : Setting {
    private string _value;
    public StringSetting(string settingId) : base(settingId) {}
    public override void SetStringValue(string str) {
        _value = str;
    }
    protected override string GetStringValue() {
        return _value ?? string.Empty;
    }
    public string Get() => _value;
    public void Set(string value) => _value = value;
}

public class IntSetting : Setting {
    private int _value;
    public IntSetting(string settingId) : base(settingId) {}
    public override void SetStringValue(string str) {
        _value = int.Parse(str);
    }
    protected override string GetStringValue() {
        return _value.ToString();
    }
    public int Get() => _value;
    public void Set(int value) => _value = value;
}

public class IntArraySetting : Setting {
    private int[] _array;
    public IntArraySetting(string settingId) : base(settingId) {}
    public override void SetStringValue(string str) {
        string[] stringNumbersArray = str.Split(' ');
        _array = new int[stringNumbersArray.Length];
        for (int i = 0; i < _array.Length; i++) {
            _array[i] = int.Parse(stringNumbersArray[i]);
        }
    }
    protected override string GetStringValue() {
        return string.Join(' ', _array);
    }
    public int[] Get() => _array;
    public void Set(int[] value) => _array = value;
    public Vector2 ToVector2() => new Vector2(_array[0], _array[1]);
}
