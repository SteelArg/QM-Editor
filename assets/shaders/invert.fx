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

sampler2D SpriteTextureSampler = sampler_state {
	Texture = <SpriteTexture>;
};

// Data structures
struct VertexShaderOutput {
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

// Fragment shader
float4 MainPS(VertexShaderOutput input) : COLOR {
	float4 c = tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
	return float4(1.0, 1.0, 1.0, c.a) - float4(c.rgb, 0.0);
}

// Technique
technique SpriteDrawing {
	pass P0 {
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};