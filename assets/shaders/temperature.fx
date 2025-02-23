// QMEDITOR USER VARIABLE 1000.0 40000.0

// Defines
#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Global variables
Texture2D SpriteTexture;

extern float UserVariable;

sampler2D SpriteTextureSampler = sampler_state {
	Texture = <SpriteTexture>;
};

// Data structures
struct VertexShaderOutput {
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

// Temperature to Color
float3 TemperatureToColor(float temperature) {
    float3 color;
    
    temperature = clamp(temperature, 1000.0, 40000.0) / 100.0;
    
    if (temperature <= 66.0)
    {
        color.r = 1.0;
        color.g = saturate(0.39008157876901960784 * log(temperature) - 0.63184144378862745098);
    }
    else
    {
        float t = temperature - 60.0;
        color.r = saturate(1.29293618606274509804 * pow(t, -0.1332047592));
        color.g = saturate(1.12989086089529411765 * pow(t, -0.0755148492));
    }
    
    if (temperature >= 66.0)
        color.b = 1.0;
    else if(temperature <= 19.0)
        color.b = 0.0;
    else
        color.b = saturate(0.54320678911019607843 * log(temperature - 10.0) - 1.19625408914);

    return color;
}

// Fragment shader
float4 MainPS(VertexShaderOutput input) : COLOR {
	float4 texColor = tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
    float3 tempColor = TemperatureToColor(UserVariable);

    float blendFactor = 0.4;
    float3 blendedColor = saturate(lerp(texColor.rgb, texColor.rgb * tempColor, blendFactor));
    return float4(blendedColor.rgb, texColor.a);
}

// Technique
technique SpriteDrawing {
	pass P0 {
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};