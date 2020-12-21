//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

float4 _NormalModUV;
TEXTURE2D(_NormalModBuffer); 
SAMPLER(sampler_NormalModBuffer);
float4 _NormalModBuffer_TexelSize;

float3 SampleNormals(float4 uvs) 
{
	float3 n1 = UnpackNormal(SAMPLE_TEX(_BumpMap, sampler_BumpMap, uvs.xy));
	float3 n2 = UnpackNormal(SAMPLE_TEX(_BumpMap, sampler_BumpMap, uvs.zw));

	return BlendNormal(n1, n2);
}

struct Normals
{
	float3 world;
	float3 wtangent;
	float3 wbitangent;
};

Normals GetNormals(float3 normal, float3 tangent, float3 bitangent)
{
	Normals n = (Normals)0;

	n.world = normalize(normal.xyz);
	n.wtangent = tangent;
	n.wbitangent = bitangent;
}

float2 GetBufferUV(float3 wPos)
{
	float2 uv = _NormalModUV.xy / _NormalModUV.z + (_NormalModUV.z / (_NormalModUV.z * _NormalModUV.z)) * wPos.xz;

	//Since version 7.3.1, UV must be flipped
#if VERSION_GREATER_EQUAL(7,4)
	//uv.y = 1 - uv.y;
#endif

	return uv;
}

float4 SampleNormalModifier(float3 wPos) 
{
	//return float4(0.5, 0.5, 1, 0);

	
	float4 buffer = SAMPLE_TEX(_NormalModBuffer, Clamp, GetBufferUV(wPos));

	float alpha = buffer.a;
	float4 normals = buffer;

	//Actually, doesn't need scaling!
	//normals.xy = normals.xy * 2.0 - 1.0;
	//normals.z = sqrt(1.0 - saturate(dot(normals.xy, normals.xy)));

	//normals.xz *= 300;

	normals = normalize(normals);

	return float4(normals.rgb, alpha);
}