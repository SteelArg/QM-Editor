using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace QMEditor.Model;

public class WorldEffectManager : Singleton<WorldEffectManager> {

    private const string UserVariableConstraintsStr = "// QMEDITOR USER VARIABLE ";

    public static WorldEffect CurrentEffect { get { return Instance?._effect; } }
    public static Action<WorldEffect> EffectChanged;

    private WorldEffect _effect;

    public WorldEffectManager() {}

    public static void LoadEffect(string path, float userVariable = 0f) {
        if (!Path.Exists(path)) {
            ServiceLocator.MessageWindowsService.ErrorWindow($"Failed to load shader.\nFile {path} does not exist.");
            ServiceLocator.LoggerService.Warning($"Failed to load shader.\nFile {path} does not exist.");
            return;
        }
        
        float[] userVariableConstraints = null;
        foreach (string line in File.ReadAllLines(path)) {
            if (line.StartsWith(UserVariableConstraintsStr)) {
                string[] nums = line.Substring(UserVariableConstraintsStr.Length).Split(' ');
                userVariableConstraints = new float[nums.Length];
                for (int i = 0; i < nums.Length; i++) {
                    userVariableConstraints[i] = float.Parse(nums[i], NumberStyles.Any, CultureInfo.InvariantCulture);
                }
            }
        }

        byte[] byteCode = Instance.CompileEffect(path);
        Instance._effect = new WorldEffect(path, byteCode, userVariable, userVariableConstraints);
        EffectChanged?.Invoke(Instance._effect);
    }

    public static void ClearEffect() {
        Instance._effect = null;
        EffectChanged?.Invoke(null);
    }

    private byte[] CompileEffect(string shaderPath) {
        Assembly mgfxc = Assembly.LoadFrom("mgfxc");

        string compiledShaderPath = $"{Path.GetDirectoryName(shaderPath)}\\{Path.GetFileNameWithoutExtension(shaderPath)}_compiled.txt";
        object output = mgfxc.EntryPoint.Invoke(null, [new string [] {shaderPath, compiledShaderPath, "/Profile:OpenGL", "/Debug"}]);
        ServiceLocator.LoggerService.Log($"Shader compilation result:\n{shaderPath}\n{output}");

        return File.ReadAllBytes(compiledShaderPath);
    }

}