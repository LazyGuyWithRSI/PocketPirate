//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

TEXTURE2D(_WaterProjectorDiffuse);
SAMPLER(sampler_WaterProjectorDiffuse);
float4 _WaterProjectorUV;

float SampleIntersection(float2 uv, float gradient, float2 time)
{
#if !_SHARP_INERSECTION && !_SMOOTH_INTERSECTION
	float inter = 0;
#endif

#if _SHARP_INERSECTION
	float sine = sin(time.y * 10 - (gradient * _IntersectionRippleDist)) * _IntersectionRippleStrength;
	float2 nUV = float2(uv.x, uv.y) * _IntersectionTiling;
	float noise = SAMPLE_TEX(_IntersectionNoise, sampler_IntersectionNoise, nUV + time.xy).r;

	float dist = saturate(gradient / _IntersectionFalloff);
	noise = saturate((noise + sine) * dist + dist);
	float inter = step(_IntersectionClipping, noise);
#endif

#if _SMOOTH_INTERSECTION
	float noise1 = SAMPLE_TEX(_IntersectionNoise, sampler_IntersectionNoise, (float2(uv.x, uv.y) * _IntersectionTiling) + (time.xy )).r;
	float noise2 = SAMPLE_TEX(_IntersectionNoise, sampler_IntersectionNoise, (float2(uv.x, uv.y) * (_IntersectionTiling * 1.5)) - (time.xy )).r;

	float dist = saturate(gradient / _IntersectionFalloff);
	float inter = saturate(noise1 + noise2 + dist) * dist;
#endif

	return saturate(inter);
}

float SampleFoam(float4 uvs, float clipping, float mask)
{
#if _FOAM
	float f1 = SAMPLE_TEX(_FoamTex, sampler_FoamTex, uvs.xy).r;
	float f2 = SAMPLE_TEX(_FoamTex, sampler_FoamTex, uvs.zw).r;

	float foam = saturate(f1 + f2) * mask;
	foam = smoothstep(clipping, 1, foam);

	return foam ;
#else
	return 0;
#endif
}

//Specular reflection in world-space
float4 SunSpecular(Light light, float3 viewDir, float3 normalWS, float perturbation, float size, float intensity)
{
	//return LightingSpecular(1, light.direction, normalWS, viewDir, 1, lerp(8196, 64, size));
	
	float3 viewLightTerm = normalize(light.direction + (normalWS * perturbation) + viewDir);
	
	float NdotL = saturate(dot(viewLightTerm, float3(0, 1, 0)));

	half specSize = lerp(8196, 64, size);
	float specular = (pow(NdotL, specSize));
	//Mask by shadows if available
	specular *= (light.distanceAttenuation * light.shadowAttenuation);

	float3 specColor = specular * light.color * intensity;

	return float4(specColor, specSize);
}

float3 SampleCaustics(float3 depthPos, float tiling, float2 pixelOffset)
{
	//Sun projection coords
	//float4 lightSpaceUVs = mul(_MainLightWorldToShadow[0], float4(DepthPos.xyz, 1)) ;

	//Planar depth projection
	float2 causticUV = (depthPos.xz * tiling) * 0.5 + (pixelOffset.xy);
	float3 caustics = SAMPLE_TEX(_CausticsTex, Repeat, causticUV).rgb;

#if _ADVANCED_SHADING
	caustics *= GetShadows(depthPos);
#endif

	return caustics;
}

float2 BoundsToWorldUV(in float3 wPos, in float4 b)
{
	float2 uv = b.xy / b.z + (b.z / (b.z * b.z)) * wPos.xz;

	//TODO: Check if required per URP version
	uv.y = 1 - uv.y;

	return uv;
}

float BoundsEdgeMask(float2 position)
{
	float2 xz = abs(position * 14.0) - 6.0;
	float pos = length(max(xz, 0));
	float neg = min(max(xz.x, xz.y), 0);
	return  1-saturate(pos + neg);
}

//#define RECEIVE_PROJECTORS

float4 SampleDiffuseProjectors(float3 wPos)
{
#ifdef RECEIVE_PROJECTORS
	float2 uv = BoundsToWorldUV(wPos, _WaterProjectorUV);

	float4 sample = SAMPLE_TEX(_WaterProjectorDiffuse, sampler_WaterProjectorDiffuse, uv);
	sample.a *= BoundsEdgeMask(uv - 0.5);
	
	return sample;
#else
	return 0;
#endif
}