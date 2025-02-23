using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class WorldEffect : Effect {
    
    private float _userVariable;
    private readonly float[] _userVariableConstraints;
    private readonly string _shaderPath;

    public WorldEffect(string shaderPath, byte[] byteCode, float defaultUserVariable=0f, float[] userVariableConstraints=null) : base(Global.Game.GraphicsDevice, byteCode) {
        _shaderPath = shaderPath;
        _userVariable = defaultUserVariable;
        SetUserVariable(defaultUserVariable);
        _userVariableConstraints = userVariableConstraints;
    }

    public void SetUserVariable(float userVariable) {
        _userVariable = userVariable;
        SetParameter("UserVariable", userVariable);
    }

    public float GetUserVariable() => _userVariable;
    public float[] GetUserVariableConstraints() => _userVariableConstraints;

    public string GetShaderPath() => $"assets\\{Path.GetRelativePath("assets", _shaderPath)}";

    public void SetParameter<T>(string name, T value) {
        EffectParameter parameter = Parameters[name];
        if (parameter == null) {
            ServiceLocator.LoggerService.Warning($"Parameter {name} does not exists in this effect.");
            return;
        }

        MethodInfo setValueOverload = parameter.GetType().GetMethod(nameof(parameter.SetValue), [typeof(T)]);
        setValueOverload.Invoke(parameter, [value]);
    }

}