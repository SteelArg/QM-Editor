using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QMEditor.Model;

public class InspectionData {

    public dynamic InspectedObject { get => _inspectedObject; }
    
    private readonly dynamic _inspectedObject;
    private readonly Type _type;
    private List<InspectionProperty> _properties = new List<InspectionProperty>();
    private string _objectName = "Unnamed";

    public InspectionData(dynamic inspectedObject) {
        _inspectedObject = inspectedObject;

        _type = inspectedObject.GetType();
        PropertyInfo[] inspectedProperties = _type.GetProperties().Where(p => Attribute.IsDefined(p, typeof(AddToInspectionAttribute))).ToArray();
        foreach (PropertyInfo property in inspectedProperties) {
            var inspectionAttribute = (AddToInspectionAttribute)property.GetCustomAttribute(typeof(AddToInspectionAttribute));
            AddProperty(inspectionAttribute.CreateInspectionProperty(inspectedObject, property));
        }
    }

    public void AddProperty(InspectionProperty property) {
        _properties.Add(property);
    }

    public InspectionProperty[] GetProperties() => _properties.ToArray();

    public void SetName(string objectName) {
        objectName = objectName ?? string.Empty;
        _objectName = objectName == string.Empty ? "Unnamed" : objectName;
        string upperCasedName = string.Empty;
        foreach (string word in _objectName.Replace('_', ' ').Split(' ')) {
            upperCasedName += word[0].ToString().ToUpper() + word.Substring(1) + " ";
        }
        _objectName = upperCasedName.Remove(upperCasedName.Length - 1);
    }

    public string GetName() {
        return $"{_type.Name} {_objectName}";
    }

}
