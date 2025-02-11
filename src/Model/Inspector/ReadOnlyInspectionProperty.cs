using System.Reflection;

namespace QMEditor.Model;

public class ReadOnlyInspectionProperty : InspectionProperty {

    public ReadOnlyInspectionProperty(object inspectionObject, PropertyInfo property, PropertyType type) : base(inspectionObject, property, type) {}

    public override void SetValue(object value) {
        // noop
    }

    public override bool IsEditable() => false;

}
