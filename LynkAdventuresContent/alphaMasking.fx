/*
	Author: Denis Zhidkikh
	Basic alpha masking.
	Best result when using BlendState.NonPremultiplied
*/

uniform extern texture MainTexture;
sampler MainTextureSampler = sampler_state
{
	Texture = <MainTexture>;
};

uniform extern texture AlphaMask;
sampler AlphaMaskSampler = sampler_state
{
	Texture = <AlphaMask>;
};

static float multiplier = (1.0 / 32) / (1.0 / 256);
float4 PixelShaderFunction(float2 pixPos: TEXCOORD0) : COLOR
{
	float4 MainTextureCol = tex2D(MainTextureSampler, pixPos);
	MainTextureCol.a = MainTextureCol.a - tex2D(AlphaMaskSampler, pixPos * multiplier).r;

    return MainTextureCol;
}

technique
{
    pass P0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
