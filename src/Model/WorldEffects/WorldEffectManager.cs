using System;
using System.IO;
using System.Reflection;

namespace QMEditor.Model;

public class WorldEffectManager : Singleton<WorldEffectManager> {

    public static WorldEffect CurrentEffect { get { return Instance?._effect; } }

    private WorldEffect _effect;

    public WorldEffectManager() {}

    public static void LoadEffect(string path) {
        if (!Path.Exists(path)) {
            ServiceLocator.MessageWindowsService.ErrorWindow($"Failed to load shader.\nFile {path} does not exist.");
            return;
        }
        Instance._effect = Instance.CompileEffect(path);
    }

    private WorldEffect CompileEffect(string shaderPath) {
        Assembly mgfxc = Assembly.LoadFrom("mgfxc");

        string compiledShaderPath = $"{Path.GetDirectoryName(shaderPath)}\\{Path.GetFileNameWithoutExtension(shaderPath)}_compiled.txt";
        object output = mgfxc.EntryPoint.Invoke(null, [new string [] {shaderPath, compiledShaderPath, "/Profile:OpenGL", "/Debug"}]);
        Console.WriteLine(output);

        return new WorldEffect(File.ReadAllBytes(compiledShaderPath));
    }

}