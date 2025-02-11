using System;
using System.Reflection;

namespace QMEditor.Model;

[AttributeUsage(AttributeTargets.Property)]
public class AddToInspectionAttribute : Attribute {
    
    private InspectionProperty _property;
    private InspectionProperty.PropertyType _propertyType;
    private AccessMode _accesMode;

    public AddToInspectionAttribute(InspectionProperty.PropertyType propertyType, AccessMode accesMode = AccessMode.GetSet) {
        _propertyType = propertyType;
        _accesMode = accesMode;
    }

    public InspectionProperty CreateInspectionProperty(object inspectionObject, PropertyInfo property) {
        switch (_accesMode) {
            case AccessMode.GetSet:
                _property = new InspectionProperty(inspectionObject, property, _propertyType);
                break;
            case AccessMode.ReadOnly:
                _property = new ReadOnlyInspectionProperty(inspectionObject, property, _propertyType);
                break;
        }

        return _property;
    }

    public enum AccessMode {
        GetSet,
        ReadOnly
    }

}
