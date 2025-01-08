namespace QMEditor;

public class Settings : Singleton<Settings> {

    public static Setting<float> Volume {get => Instance._volume;}

    private Setting<float> _volume;

    protected override void OnSingletonCreated() {
        _volume = new Setting<float>("volume", 1f);
    }

    public static void Save() {
        // Write all settings in a string
        // Save to a file
    }

    public static void Load() {
        // Read from a file
        // Set values to settings
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

    public string GetSaveString() {
        return $"{_settingId}={ToStr()}";
    }

    public string ToStr() {
        return _value.ToString();
    }

}