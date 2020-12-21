//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

struct WaveInfo
{
	float3 position;
	float3 normal;
};

half3 GerstnerOffset4(half2 xzVtx, half4 steepness, half4 amp, half4 freq, half4 speed, half4 dirAB, half4 dirCD)
{
	half3 offsets;

	half4 AB = steepness.xxyy * dirAB.xyzw * amp.xxyy;
	half4 CD = steepness.zzww * dirCD.xyzw * amp.zzww;

	half4 dotABCD = freq.xyzw * half4(dot(dirAB.xy, xzVtx), dot(dirAB.zw, xzVtx), dot(dirCD.xy, xzVtx), dot(dirCD.zw, xzVtx));

	half4 COS = cos(dotABCD + speed);
	half4 SIN = sin(dotABCD + speed);

	offsets.x = dot(COS, half4(AB.xz, CD.xz));
	offsets.z = dot(COS, half4(AB.yw, CD.yw));
	offsets.y = dot(SIN, amp); //Remap to only positive values;

	return offsets;
}

half3 GerstnerNormal4(half2 xzVtx, half4 amp, half4 freq, half4 speed, half4 dirAB, half4 dirCD)
{
	half3 nrml = half3(0, 2.0, 0);

	half4 AB = freq.xxyy * amp.xxyy * dirAB.xyzw;
	half4 CD = freq.zzww * amp.zzww * dirCD.xyzw;

	half4 dotABCD = freq.xyzw * half4(dot(dirAB.xy, xzVtx), dot(dirAB.zw, xzVtx), dot(dirCD.xy, xzVtx), dot(dirCD.zw, xzVtx));

	half4 COS = cos(dotABCD + speed);

	nrml.x -= dot(COS, half4(AB.xz, CD.xz));
	nrml.z -= dot(COS, half4(AB.yw, CD.yw));

	nrml.xz *= _WaveNormalStr;
	nrml = normalize(nrml);

	return nrml;
}

void Gerstner(inout half3 offs, inout half3 nrml,
	half2 position,
	half4 amplitude, half4 frequency, half4 steepness,
	half4 speed, half4 directionAB, half4 directionCD)
{
	offs += GerstnerOffset4(position, steepness, amplitude, frequency, speed, directionAB, directionCD);
	#if !_FLAT_SHADING || !defined(VERTEX_PASS) //Normals are per face
	nrml += GerstnerNormal4(position, amplitude, frequency, speed, directionAB, directionCD);
	#endif
}

#define WAVE_COUNT _WaveCount
#define MAX_WAVE_COUNT 5

WaveInfo GetWaveInfo(float2 position, float2 time)
{
	WaveInfo waves = (WaveInfo)0;

	float4 amp = float4(0.3, 0.35, 0.25, 0.25);
	float4 freq = float4(1.3, 1.35, 1.25, 1.25) * (1-_WaveDistance) * 3;
	float4 speed = float4(1.2* time.x, 1.375* time.y, 1.1* time.x, 1* time.y) ; //Pre-multiplied with time
	float4 dir1 = float4(0.3, 0.85, 0.85, 0.25) * _WaveDirection;
	float4 dir2 = float4(0.1, 0.9, -0.5, -0.5) * _WaveDirection;
	float4 steepness = float4(12.0, 12.0, 12.0, 12.0) * _WaveSteepness * lerp(1, MAX_WAVE_COUNT, 1/WAVE_COUNT);

	for (uint i = 0; i <= WAVE_COUNT; i++)
	{
		float t = 1+((float)i / (float)WAVE_COUNT);
		freq *= t;
		
		Gerstner(/*out*/ waves.position, /*out*/ waves.normal, position, amp, freq, steepness, speed, dir1, dir2);
	}

	waves.normal = normalize(waves.normal);
	//Average
	waves.position.y /= WAVE_COUNT;
	//waves.normal.xz *= WAVE_COUNT;

	return waves;
}