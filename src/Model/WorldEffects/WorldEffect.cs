using System;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class WorldEffect : Effect {
    
    private float _userVariable;
    private readonly string _shaderPath;

    public WorldEffect(string shaderPath, byte[] byteCode, float defaultUserVariable=0f) : base(Global.Game.GraphicsDevice, byteCode) {
        _shaderPath = shaderPath;
        _userVariable = defaultUserVariable;
        SetUserVariable(defaultUserVariable);
        // SetParameter("MatrixTransform", Matrix.CreateOrthographicOffCenter(0, Resolution.Current[0], Resolution.Current[1], 0, -2000.0f, 2000.0f));
    }

    public void SetUserVariable(float userVariable) {
        _userVariable = userVariable;
        SetParameter("UserVariable", userVariable);
    }

    public float GetUserVariable() => _userVariable;

    public string GetShaderPath() => $"assets\\{Path.GetRelativePath("assets", _shaderPath)}";

    public void SetParameter<T>(string name, T value) {
        EffectParameter parameter = Parameters[name];
        if (parameter == null) {
            Console.WriteLine($"Parameter {name} does not exists in this effect.");
            return;
        }

        MethodInfo setValueOverload = parameter.GetType().GetMethod(nameof(parameter.SetValue), [typeof(T)]);
        setValueOverload.Invoke(parameter, [value]);
    }

}