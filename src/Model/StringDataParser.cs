using System.Collections.Generic;

namespace QMEditor.Model;

public class StringDataParser {

    private Dictionary<string, string> _data;
    private string _filename;

    public StringDataParser(string filename) {
        _data = new Dictionary<string, string>();
        _filename = filename;
    }

    public void SetValue(string name, string value) {
        if (_data.ContainsKey(name))
            _data[name] = value;
        else
            AddValue(name, value);
    }

    public string GetValue(string name) {
        return _data[name];
    }

    public void Ensure(string name, string value = "0") {
        if (!_data.ContainsKey(name))
            AddValue(name, value);
    }

    private void AddValue(string name, string value) => _data.Add(name, value);

    public void Load() {
        string buffer = ServiceLocator.FileService.Read(_filename);
        _data.Clear();

        foreach (string line in buffer.Split("\n")) {
            if (line == string.Empty) continue;

            string name = line.Split('=')[0];
            string value = line.Split('=')[1];
            
            _data.Add(name, value);
        }
    }

    public void Save() {
        string buffer = "";
        foreach (string key in _data.Keys) {
            buffer += key + "=" + _data[key] + "\n";
        }

        ServiceLocator.FileService.Write(_filename, buffer);
    }
}