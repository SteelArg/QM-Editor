using System.Reflection;

namespace QMEditor.Model;

public class InspectionProperty {
    
    public PropertyType Type { get => _type; }

    private PropertyType _type;
    protected PropertyInfo _inspectedProperty;
    protected object _inspectedObject;

    public InspectionProperty(object inspectionObject, PropertyInfo property, PropertyType type) {
        _inspectedObject = inspectionObject;
        _inspectedProperty = property;
        _type = type;
    }

    public virtual object GetValue() {
        return _inspectedProperty.GetValue(_inspectedObject);
    }

    public virtual void SetValue(object value) {
        _inspectedProperty.SetValue(_inspectedObject, value);
    }

    public virtual string GetName() => _inspectedProperty.Name;
    public virtual bool IsEditable() => true;

    public enum PropertyType {
        Integer,
        String,
        Check,
        GridPosition,
        InspectableLink,
        InspectablesLinkArray,
        Color
    }

}
