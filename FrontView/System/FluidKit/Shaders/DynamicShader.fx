
sampler2D implicitInput : register(s0);
float shaderIndex : register(c0);
float param1 : register(c1);
float param2 : register(c2);
float param3 : register(c3);

float2 Crystallize(float2 inCoord)
{
	float center = (0.5, 0.5);
	float delta = center - inCoord;
	float distance = length(delta);
	
	float amplitude = param1;
	float xFreq = param2;
	float yFreq = param3;
	
	float2 outCoord = float2(inCoord.x + cos(xFreq * inCoord.y) * amplitude, 
							inCoord.y + sin(yFreq * inCoord.x) * amplitude);
							
	return outCoord;
}


float2 BlueShift(float2 inCoord)
{
	float4 color = tex2D(implicitInput, inCoord);
	
	float shift = 0;
	if (param2 >= 0 && param2 < 0.33)
	{
		shift = color.r * param1;
	}
	
	if (param2 >= 0.33 && param2 < 0.66)
	{
		shift = color.g * param1;
	}
	
	if (param2 >= 0.66 && param2 <= 1)
	{
		shift = color.b * param1;
	}
	
	
	float2 outCoord = float2(shift + inCoord.x, shift + inCoord.y);
	
	return outCoord;
}

float4 MainPS(float2 uv : TEXCOORD) : COLOR
{
	float2 outCoord = uv;
	if (shaderIndex == 0)
		outCoord = Crystallize(uv);
	else if (shaderIndex == 1)
		outCoord = BlueShift(uv);
		
	return tex2D(implicitInput, outCoord);
}

