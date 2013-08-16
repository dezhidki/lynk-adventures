/*
Author: Denis Zhidkikh
Summary: Computes and draws modified rectangle. Modified Rectangle (therefore ModalRect) is a rectangle that has
has a texture that is bigger then a ModalRect or the same size. The rendering starting point can be changed.
*/

// ModalRect that will be drawn on the screen. (Assigned at SpriteBatch.Draw).
float tint = 0.5;
uniform extern texture MainTexture;

sampler MainTextureSampler = sampler_state
{
	Texture = <MainTexture>;
};

// Texture that will be drawn on top of the ModalRect.
uniform extern texture Image;
sampler ImageSampler = sampler_state
{

	// Filters to force SamplerState.PointWrap
	MipFilter = Point;
	MagFilter = Point;
	MinFilter = Point;
	AddressU = Wrap;
	AddressV = Wrap;
	Texture = <Image>;
};

// The point from where to start getting pixels from Image.
float2 Offset;
// Image's size.
float2 ImageSize;
// ModalRect's size.
float2 RenderSize;

// Scales pixel's position to what it would be if we rendered Image in its original size.
float2 ToFixedPixelPos(float2 pixPos)
{
	float2 sizeFactor = ImageSize / RenderSize;
		return pixPos / sizeFactor;
}

float4 PixelShaderFunction(float4 col : COLOR, float2 pixPos: TEXCOORD0) : COLOR
{
	col.a = tint;
	// Covert pixel's position to the "fixed" one. Needed to not to scale texture when ModalRect's size is different than Image's.
	pixPos = ToFixedPixelPos(pixPos);
	float2 pixPosOffs = pixPos + Offset;

	// Convert from HLSL's homogenous position to heterogenous (from range [0.0,1.0] to original image's range. [0,width] and [0,height])
	float2 imageCoords = pixPosOffs.xy * ImageSize.xy;

	// Cehck if the pixel we are going to draw is out of original Image's boundaries. Then we won't wrap it, but cut it.
	if(imageCoords.x > ImageSize.x || imageCoords.y > ImageSize.y)
		return float4(0,0,0,0);

	// Get pixels from the textures (Actually mainTextureCol's color doesn't matter, but still needed to properly render the pixel
	float4 mainTextureCol = tex2D(MainTextureSampler, pixPos);
	float4 imageTexCol = tex2D(ImageSampler, pixPosOffs);

	//imageTexCol.rgb *= imageTexCol.a
	mainTextureCol.rgb = imageTexCol.rgb;

	if(col.r != 1.0 || col.g != 1.0 || col.b != 1.0)
	{
		col.rgb *= col.a;
		mainTextureCol.rgb = col.rgb + (mainTextureCol.rgb * (1 - col.a));
	}

	// Blend alpha value (Premultiplied alpha style)

	// Premultiply RGB values
	mainTextureCol.rgb *= mainTextureCol.a;

	mainTextureCol.a *= imageTexCol.a;
	return mainTextureCol;
}

technique
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
