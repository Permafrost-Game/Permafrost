#if OPENGL
	#define PS_SHADERMODEL ps_3_0
#else
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Effect applies normalmapped lighting to a 2D sprite.


float3 LightDirection;
Texture2D ScreenTexture;
Texture2D NormalTexture;

float3 LightColor = 1.0;
float3 AmbientColor = 0.35;



SamplerState TextureSampler = sampler_state
{
	Texture = <ScreenTexture>;
};

SamplerState NormalSampler = sampler_state
{
	Texture = <NormalTexture>;
};

float4 main(float4 pos : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{
	//Look up the texture value
	float4 tex = ScreenTexture.Sample(TextureSampler, texCoord);

	//Look up the normalmap value
	float4 normal = 2 * NormalTexture.Sample(NormalSampler, texCoord) - 1;

	// Compute lighting.
	float lightAmount = saturate(dot(normal.xyz, LightDirection)) + 0.5f;
	color.rgb *= AmbientColor + (lightAmount * LightColor);

	return color * tex;
}

technique Normalmap
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL main();
	}
}