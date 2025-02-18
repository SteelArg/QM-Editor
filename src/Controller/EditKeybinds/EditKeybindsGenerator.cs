using System.Collections.Generic;

namespace QMEditor.Controllers;

public interface IEditKeybindsGenerator {

    public List<EditKeybind> Generate();

}