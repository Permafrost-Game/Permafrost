/*  Copyright (C) 2011 by Catalin Zima-Zegreanu

    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”),
    to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
    and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
    WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
// This shader was created by Catalin ZZ 
// Source http://www.catalinzima.com/xna/samples/shader-based-dynamic-2d-smooth-shadows/


texture SourceTexture; 
          
sampler inputSampler = sampler_state      
{
            Texture   = <SourceTexture>;
            
            MipFilter = Point;
            MinFilter = Point;
            MagFilter = Point;
            
            AddressU  = Clamp;
            AddressV  = Clamp;
};
   
float2 TextureDimensions;

struct VS_OUTPUT
{
    float4 Pos  : POSITION;
    float2 Tex  : TEXCOORD0;
};

VS_OUTPUT VS(
    float3 InPos  : POSITION,
    float2 InTex  : TEXCOORD0)
{
    VS_OUTPUT Out = (VS_OUTPUT)0;

    // transform the position to the screen
    Out.Pos = float4(InPos,1) + float4(-TextureDimensions.x, TextureDimensions.y, 0, 0);
    Out.Tex = InTex;

    return Out;
}


float4 HorizontalReductionPS(float2 TexCoord  : TEXCOORD0) : COLOR0
{
	  float2 color = tex2D(inputSampler, TexCoord);
	  float2 colorR = tex2D(inputSampler, TexCoord + float2(TextureDimensions.x,0));
	  float2 result = min(color,colorR);
      return float4(result,0,1);
}

float4 CopyPS(float2 TexCoord  : TEXCOORD0) : COLOR0
{
	return tex2D(inputSampler, TexCoord);
}


technique HorizontalReduction
{
    pass P0
    {          
        VertexShader = compile vs_3_0 VS();
        PixelShader  = compile ps_3_0 HorizontalReductionPS();
    }
}

technique Copy
{
    pass P0
    {          
        VertexShader = compile vs_3_0 VS();
        PixelShader  = compile ps_3_0 CopyPS();
    }
}