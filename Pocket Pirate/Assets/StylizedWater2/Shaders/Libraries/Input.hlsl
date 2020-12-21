//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

TEXTURE2D(_IntersectionNoise);
SAMPLER(sampler_IntersectionNoise);
TEXTURE2D(_CausticsTex);
TEXTURE2D(_DepthTex);
TEXTURE2D(_FoamTex);
SAMPLER(sampler_FoamTex);
TEXTURE2D(_PlanarReflectionLeft);
SAMPLER(sampler_PlanarReflectionLeft);
TEXTURE2D(_PlanarReflectionRight);
SAMPLER(sampler_PlanarReflectionRight);


CBUFFER_START(UnityPerMaterial)
	float4 _ShallowColor;
	float4 _BaseColor; 
	float4 _IntersectionColor;
	float _Depth;
	float _DepthExp;
	float _WorldSpaceUV;
	float _NormalTiling;
	half _NormalStrength;
	half4 _TranslucencyParams;
	half _EdgeFade;
	float _WaveSpeed;
	float4 _HorizonColor;
	half _HorizonDistance;
	float _SparkleIntensity;
	half _SparkleSize;
	float _SunReflectionDistortion;
	float _SunReflectionSize;
	float _SunReflectionStrength;
	float _ReflectionDistortion;
	float _ReflectionBlur;
	float _ReflectionStrength;
	half _PlanarReflectionsParams;
	half _PlanarReflectionsEnabled;
	half _ShadowStrength;
	half4 _AnimationParams;

	//Foam
	half _FoamTiling;
	float4 _FoamColor;
	half _FoamSpeed;
	half _FoamSize;
	half _FoamWaveMask;
	half _FoamWaveMaskExp;

	//Intersection
	half _IntersectionSource;
	half _IntersectionLength;
	half _IntersectionFalloff;
	half _IntersectionTiling;
	half _IntersectionRippleDist;
	half _IntersectionRippleStrength;
	half _IntersectionClipping;
	half _IntersectionSpeed;

	//Waves
	half _WaveHeight;
	half _WaveNormalStr;
	half _WaveDistance;
	half _WaveSteepness;
	uint _WaveCount;
	half4 _WaveDirection;

	half _ShoreLineWaveStr;
	half _ShoreLineWaveDistance;
	half _ShoreLineLength;

	//Underwater
	half _CausticsBrightness;
	half _CausticsTiling;
	half _CausticsDistortion;
	half _RefractionStrength;

	half4 _VertexColorMask;
	half _DepthMode;
	float4 _DepthMapBounds;
	half _WaveTint;
#ifdef TESSELLATION_ON	
	float _TessValue;
	float _TessMin;
	float _TessMax;

#endif
CBUFFER_END