using System;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace QMEditor.Model;

public class WorldEffect : Effect {
    
    private float _userVariable;

    public WorldEffect(byte[] byteCode, float defaultUserVariable=0f) : base(Global.Game.GraphicsDevice, byteCode) {
        _userVariable = defaultUserVariable;
        SetUserVariable(defaultUserVariable);
    }

    public void SetUserVariable(float userVariable) {
        _userVariable = userVariable;
        SetParameter<float>("UserVariable", userVariable);
    }

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