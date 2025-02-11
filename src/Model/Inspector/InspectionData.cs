using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QMEditor.Model;

public class InspectionData {

    private dynamic _inspectedObject;
    private Type _type;
    private List<InspectionProperty> _properties = new List<InspectionProperty>();

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

    public string GetName() {
        return $"{_type.Name}";
    }

}
