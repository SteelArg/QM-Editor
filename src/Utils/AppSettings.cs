using QMEditor.Model;

namespace QMEditor;

public class AppSettings : Singleton<AppSettings> {

    private const string SettingsPath = "assets\\settings\\settings.txt";

    public static Setting<float> Volume {get => Instance._volume;}

    private Setting<float> _volume;
    private StringDataParser _dataParser;

    public AppSettings() : base() {
        _volume = new Setting<float>("volume", 1f);
        _dataParser = new StringDataParser(SettingsPath);
        Load();
    }

    public void Save() {
        _dataParser.SetValue("volume", _volume.ToString());
        _dataParser.Save();
    }

    public void Load() {
        _dataParser.Load();
        _dataParser.Ensure("volume", 1f.ToString());
        _volume.SetValue(float.Parse(_dataParser.GetValue("volume")));
    }

}

public class Setting<T> {

    private string _settingId;
    private T _value;

    public T Value {get => _value;}

    public Setting(string settingId, T value) {
        _settingId = settingId;
        _value = value;
    }

    public override string ToString() {
        return _value.ToString();
    }

    public void SetValue(T value) {
        _value = value;
    }

}