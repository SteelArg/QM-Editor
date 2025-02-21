using System;
using System.IO;
using System.Reflection;

namespace QMEditor.Model;

public class WorldEffectManager : Singleton<WorldEffectManager> {

    public static WorldEffect CurrentEffect { get { return Instance?._effect; } }
    public static Action<WorldEffect> EffectChanged;

    private WorldEffect _effect;

    public WorldEffectManager() {}

    public static void LoadEffect(string path, float userVariable = 0f) {
        if (!Path.Exists(path)) {
            ServiceLocator.MessageWindowsService.ErrorWindow($"Failed to load shader.\nFile {path} does not exist.");
            return;
        }
        Instance._effect = Instance.CompileEffect(path, userVariable);
        EffectChanged?.Invoke(Instance._effect);
    }

    public static void ClearEffect() {
        Instance._effect = null;
        EffectChanged?.Invoke(null);
    }

    private WorldEffect CompileEffect(string shaderPath, float userVariable = 0f) {
        Assembly mgfxc = Assembly.LoadFrom("mgfxc");

        string compiledShaderPath = $"{Path.GetDirectoryName(shaderPath)}\\{Path.GetFileNameWithoutExtension(shaderPath)}_compiled.txt";
        object output = mgfxc.EntryPoint.Invoke(null, [new string [] {shaderPath, compiledShaderPath, "/Profile:OpenGL", "/Debug"}]);
        Console.WriteLine(output);

        return new WorldEffect(shaderPath, File.ReadAllBytes(compiledShaderPath), userVariable);
    }

}