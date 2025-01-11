namespace QMEditor;

public class AppSettings : Singleton<AppSettings> {

    private const string SettingsPath = "assets\\settings\\settings.txt";

    public static Setting<float> Volume {get => Instance._volume;}

    private Setting<float> _volume;

    public AppSettings() : base() {
        _volume = new Setting<float>("volume", 1f);
        Load();
    }

    public static void Save() {
        string settings = Instance._volume.GetSaveString();
        ServiceLocator.FileService.Write(SettingsPath, settings);
    }

    public static void Load() {
        string settings = ServiceLocator.FileService.Read(SettingsPath);

        foreach (string line in settings.Split(" ")) {
            if (line == string.Empty) continue;

            string name = line.Split('=')[0];
            string value = line.Split('=')[1];
            
            switch (name) {
                case "volume":
                    Instance._volume.SetValue(float.Parse(value)); break;
            }
        }
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

    public void SetValue(T value) {
        _value = value;
    }

}