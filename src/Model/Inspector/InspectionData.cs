using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QMEditor.Model;

public class InspectionData {

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
        _objectName = _objectName[0].ToString().ToUpper() + _objectName.Substring(1);
    }

    public string GetName() {
        return $"{_type.Name} {_objectName}";
    }

}
