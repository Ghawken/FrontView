
sampler2D implicitInput : register(s0);
float lp0 : register(c0);
float lp1 : register(c1);
float lp2 : register(c2);
float lp3 : register(c3);

float rp0 : register(c4);
float rp1 : register(c5);
float rp2 : register(c6);
float rp3 : register(c7);


float BezierInterpolate(float x0, float x1, float x2, float x3, float t)
{
	float b0 = pow(1-t, 3);
	float b1 = 3*t*pow(1-t,2);
	float b2 = 3*t*t*(1-t);
	float b3 = pow(t, 3);
	
	return b0*x0 + b1*x1 + b2*x2 + b3*x3;
}

float4 MainPS(float2 uv : TEXCOORD) : COLOR
{
	float left = BezierInterpolate(lp0, lp1, lp2, lp3, uv.y);
	float right = BezierInterpolate(rp0, rp1, rp2, rp3, uv.y);
	
	if (uv.x >= left && uv.x <= right)
	{
		float tx = lerp(0, 1, (uv.x-left)/(right-left));
		float2 pos = float2(tx, uv.y);
		
		return tex2D(implicitInput, pos);
	}
	else return float4(0,0,0,1);
}

