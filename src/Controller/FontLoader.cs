using System;
using System.Collections.Generic;
using System.IO;
using FontStashSharp;

namespace QMEditor.Controllers;

public class FontLoader : Singleton<FontLoader> {

    private Dictionary<FontType, FontSystem> _fonts;

    public FontLoader() {
        _fonts = new Dictionary<FontType, FontSystem>();
    }

    public void Load() {
        LoadFonts();
    }
    
    public static DynamicSpriteFont GetFont(float fontSize, FontType fontType = FontType.Main) {
        return Instance._fonts[fontType].GetFont(fontSize);
    }

    private void LoadFonts() {
        foreach (FontType fontType in Enum.GetValues(typeof(FontType))) {
            string fontName = fontType.ToString().ToLower();
            var fontSystem = new FontSystem();
            fontSystem.AddFont(File.ReadAllBytes($"assets\\fonts\\{fontName}.ttf"));
            _fonts.Add(fontType, fontSystem);
        }
    }

    public enum FontType {
        Main, Bold
    }

}