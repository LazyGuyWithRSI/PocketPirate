//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

float4x4 _SunProjection;

#define UP_VECTOR float3(0,1,0)

//Note: Throws an error about a BLENDWEIGHTS vertex attribute on GLES when VR is enabled
//Possibly related to: https://issuetracker.unity3d.com/issues/oculus-a-non-system-generated-input-signature-parameter-blendindices-cannot-appear-after-a-system-generated-value
#if UNDERWATER_ENABLED
half4 ForwardPassFragment(Varyings input, FRONT_FACE_TYPE facing : FRONT_FACE_SEMANTIC) : SV_Target
#else
half4 ForwardPassFragment(Varyings input) : SV_Target
#endif
{
	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

	float3 finalColor = 0;
	float alpha = 1;

	//0 = back face
	#if UNDERWATER_ENABLED
	facing = IS_FRONT_VFACE(facing, true, false);
	//finalColor = lerp(float3(1,0,0), float3(0,1,0), IS_FRONT_VFACE(facing, true, false));
	//return float4(finalColor.rgb, 1);
	#endif
	
	float4 vertexColor = GetVertexColor(input.color, _VertexColorMask);
	//return float4(vertexColor.rgb, 1);

	//Vertex normal in world-space
	float3 normalWS = normalize(input.normal.xyz);
#if _NORMALMAP
	float3 WorldTangent = input.tangent.xyz;
	float3 WorldBiTangent = input.bitangent.xyz;
	float3 wPos = float3(input.normal.w, input.tangent.w, input.bitangent.w);
#else
	float3 wPos = input.wPos;
#endif
	//Not normalized for depth-pos reconstruction. Normalization required for lighting (otherwise breaks on mobile)
	float3 viewDir = (_WorldSpaceCameraPos - wPos);
	float3 viewDirNorm = SafeNormalize(viewDir);
	//return float4(viewDir, 1);
	
	#if _FLAT_SHADING
	float3 dpdx = ddx(wPos.xyz);
	float3 dpdy = ddy(wPos.xyz);
	normalWS = normalize(cross(dpdy, dpdx));
	#endif

	//Returns mesh or world-space UV
	float2 uv = GetSourceUV(input.uv.xy, wPos.xz, _WorldSpaceUV);
	
	// Waves
	//----------
	float height = 0;

	float3 waveNormal = normalWS;
#if _WAVES
	WaveInfo waves = GetWaveInfo(uv, TIME * _WaveSpeed);
	#if !_FLAT_SHADING
		//Flatten by blue vertex color weight
		waves.normal = lerp(waves.normal, normalWS, lerp(0, 1, vertexColor.b));
		//Blend wave/vertex normals in world-space
		waveNormal = BlendNormalWorldspaceRNM(waves.normal, normalWS, UP_VECTOR);
	#endif
	//return float4(waveNormal.xyz, 1);
	height = waves.position.y * 0.5 + 0.5;
	height *= lerp(1, 0, vertexColor.b);
	//return float4(height, height, height, 1);

	//vertices are already displaced on XZ, in this case the world-space UV needs the same treatment
	if(_WorldSpaceUV == 1) uv.xy -= waves.position.xz * HORIZONTAL_DISPLACEMENT_SCALAR * _WaveHeight;
	//return float4(frac(uv.xy), 0, 1);
#endif


	float4 ShadowCoords = float4(0, 0, 0, 0);
	#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && !defined(UNLIT)
	ShadowCoords = input.shadowCoord;
	#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS) && !defined(UNLIT)
	ShadowCoords = TransformWorldToShadowCoord(wPos);
	#endif

	Light mainLight = GetMainLight(ShadowCoords);

	half shadowMask = 1;
	#if _ADVANCED_SHADING
	shadowMask = GetShadows(wPos.xyz);
	//return float4(shadowMask,shadowMask,shadowMask,1);
	#endif

	//Normals
//---------------------
	float3 NormalsCombined = float3(0.5, 0.5, 1);
	float3 worldTangentNormal = waveNormal;

#if _NORMALMAP
	float4 nUV = PackedUV(uv * _NormalTiling, TIME, _WaveSpeed * 0.1);
	NormalsCombined = SampleNormals(nUV);
	//float4 NormalsMod = SampleNormalModifier(wPos);
	//NormalsCombined = lerp(NormalsCombined, NormalsMod.rgb, NormalsMod.a);

	//#if !_FLAT_SHADING //Skip, not a good fit
	worldTangentNormal = normalize(TransformTangentToWorld(NormalsCombined, half3x3(WorldTangent, WorldBiTangent, waveNormal)));
	//#endif
#endif

#ifdef SCREEN_POS
	float4 ScreenPos = input.screenPos;
	//ScreenPos.z = (UNITY_NEAR_CLIP_VALUE >= 0) ? ScreenPos.z : ScreenPos.z * 0.5 + 0.5;
#else
	float4 ScreenPos = 0;
#endif
#if _REFRACTION
	float4 refractedScreenPos = ScreenPos.xyzw + (float4(worldTangentNormal.xy, 0, 0) * (_RefractionStrength * 0.1));
#endif

	DepthData depth = SampleDepth(ScreenPos, wPos);
	//return float4(depth.linear01, depth.linear01, depth.linear01, 1);

	float3 opaqueWorldPos = wPos;
	float opaqueDist = 1;
	float aborptionDist = opaqueDist;
#if !_DISABLE_DEPTH_TEX
	opaqueWorldPos = ReconstructViewPos(ScreenPos, viewDir, depth);
	//return float4(frac(opaqueWorldPos.xyz), 1);
	//
	//Invert normal when viewing backfaces
	float normalSign = ceil(dot(viewDirNorm, normalWS));
	normalSign = normalSign == 0 ? -1 : 1;
	
	opaqueDist = DepthDistance(wPos, opaqueWorldPos, normalWS * normalSign);
	//return float4(opaqueDist,opaqueDist,opaqueDist,1);
	
	#if _ADVANCED_SHADING && _REFRACTION
	//Double sample depth to avoid depth discrepancies
	//DepthData depthRefracted = SampleDepth(refractedScreenPos, wPos);
	//float3 opaqueWorldPosRefracted = ReconstructViewPos(refractedScreenPos, viewDir, depthRefracted);
	//aborptionDist = DepthDistance(wPos, opaqueWorldPosRefracted, normalWS);
	aborptionDist = opaqueDist;
#else
	aborptionDist = opaqueDist;
#endif
#endif

#if _ADVANCED_SHADING && _REFRACTION
	//Reject any offset pixels above water
	float refractionMask = saturate(wPos.y - opaqueWorldPos.y);
	//return float4(refractionMask,refractionMask,refractionMask,1);
	
	#if UNDERWATER_ENABLED
	refractionMask *= facing;
	#endif
	refractedScreenPos = lerp(ScreenPos, refractedScreenPos, refractionMask);
#endif

	float AbsorptionDepth = 1;
#if !_DISABLE_DEPTH_TEX
	AbsorptionDepth = saturate(lerp(aborptionDist / _Depth, 1-(exp(-aborptionDist) / _Depth), _DepthExp));
#endif

	//return float4(AbsorptionDepth,AbsorptionDepth,AbsorptionDepth,1);
	
	//if (_DepthMode == 1) shoreDist = SampleShoreDistance(wPos);

	float intersection = 0;
#if _SHARP_INERSECTION || _SMOOTH_INTERSECTION
	float interSecGradient = 1-saturate(exp(opaqueDist) / _IntersectionLength);
	#if _DISABLE_DEPTH_TEX
	interSecGradient = 0;
	#endif
	if (_IntersectionSource == 1) interSecGradient = vertexColor.r;
	if (_IntersectionSource == 2) interSecGradient = saturate(interSecGradient + vertexColor.r);

	intersection = SampleIntersection(uv, interSecGradient, TIME * _IntersectionSpeed);
	intersection *= _IntersectionColor.a;

	//Flatten normals on intersection foam
	waveNormal = lerp(waveNormal, normalWS, intersection);
#endif
	//return float4(intersection,intersection,intersection,1);

	float shoreSine = 0;
	/* Disabled, pretty much requires static depth info
	//Shoreline wave normals
	if (_DepthMode == 1) _ShoreLineLength = clamp(_ShoreLineLength, 0, 1);
	half shoreWaveDist = saturate(shoreDist / _ShoreLineLength);
	//shoreWaveDist = shoreWaveDist > 0.5 ? 0 : shoreWaveDist;
	//return float4(shoreWaveDist, shoreWaveDist, shoreWaveDist, 1);

	shoreSine = (sin((shoreWaveDist * _ShoreLineWaveDistance * _ShoreLineLength) + (TIME * _WaveSpeed * 0.25)) * 0.5 + 0.5) * shoreWaveDist;
	shoreSine *= _ShoreLineWaveStr * (1 - shoreWaveDist);
	height = saturate(height + shoreSine);
	//return float4(shoreSine, shoreSine, shoreSine, 1);
	*/
	
	//return float4(height, height, height, 1);

	//FOAM
	//---------------
	float foam = 0;
	#if _FOAM
	float4 foamUV = PackedUV(uv * _FoamTiling, TIME, _FoamSpeed);
	float foamWaveMask = lerp(1, saturate(height), _FoamWaveMask);
	foamWaveMask = pow(abs(foamWaveMask), _FoamWaveMaskExp);
	foam = SampleFoam(foamUV, _FoamSize, foamWaveMask) * _FoamColor.a;
	//return float4(foam, foam, foam, 1);

	#if UNDERWATER_ENABLED
	foam *= facing;
	#endif
	#endif

	//Albedo
	//---------------
	float4 baseColor = lerp(_ShallowColor, _BaseColor, AbsorptionDepth);
	baseColor.rgb += _WaveTint * saturate(height + (shoreSine * 1.0));
	
	finalColor.rgb = baseColor.rgb;
	alpha = baseColor.a;

	float3 sparkles = 0;
	#if _NORMALMAP
	float NdotL = saturate(dot(UP_VECTOR, worldTangentNormal));
	half sunAngle = saturate(dot(UP_VECTOR, mainLight.direction));
	half angleMask = saturate(sunAngle * 10); /* 1.0/0.10 = 10 */
	sparkles = saturate(step(_SparkleSize, (saturate(NormalsCombined.y) * NdotL))) * _SparkleIntensity * mainLight.color * angleMask;
	
	finalColor.rgb += sparkles.rgb;
	#endif
	//return float4(baseColor.rgb, alpha);

	half4 sunSpec = 0;
#ifndef _SPECULARHIGHLIGHTS_OFF
	float3 sunReflectionNormals = worldTangentNormal;

	#if _FLAT_SHADING //Use face normals
	sunReflectionNormals = waveNormal;
	#endif
	
	//Blinn-phong reflection
	sunSpec = SunSpecular(mainLight, viewDirNorm, sunReflectionNormals, _SunReflectionDistortion, _SunReflectionSize, _SunReflectionStrength);
	sunSpec.rgb *=  saturate((1-foam) * (1-intersection) * shadowMask); //Hide
	//sunSpec.rgb *=  (1-intersection); //Hide
#endif

	//Reflection probe
#ifndef _ENVIRONMENTREFLECTIONS_OFF
	float3 refWorldTangentNormal = lerp(waveNormal, normalize(waveNormal + worldTangentNormal), _ReflectionDistortion);

	#if _FLAT_SHADING //Skip, not a good fit
	refWorldTangentNormal = waveNormal;
	#endif
	
	float3 reflectionVector = reflect(-viewDirNorm , refWorldTangentNormal);
	float3 reflections = SampleReflections(reflectionVector, _ReflectionBlur, ScreenPos.xyzw, refWorldTangentNormal, viewDirNorm, lerp(waveNormal.xz * 0.5, worldTangentNormal.xy, _ReflectionDistortion).xy);

	half reflectionFresnel = 1-dot(viewDirNorm, normalWS);
	finalColor.rgb = lerp((finalColor.rgb), reflections, _ReflectionStrength * 1);
#endif


#if _CAUSTICS
	float3 caustics = SampleCaustics(opaqueWorldPos, _CausticsTiling, worldTangentNormal.xy * _CausticsDistortion) * _CausticsBrightness;
	//return float4(caustics, caustics, caustics, 1);

	float causticsMask = AbsorptionDepth;
	causticsMask = saturate(causticsMask + intersection);
	
	finalColor = lerp(finalColor + caustics, finalColor, causticsMask );
#endif

	///////////////////////
	// Translucency
	//////////////////////
	TranslucencyData translucencyData = (TranslucencyData)0;
#if _TRANSLUCENCY
	float waveHeight = saturate(height);
	#if !_WAVES || _FLAT_SHADING
	waveHeight = 1;
	#endif
	//Note value is subtracted
	float transmissionMask = (foam * 0.25) + (1-shadowMask); //Foam isn't 100% opaque

	//return float4(transmissionMask, transmissionMask, transmissionMask, 1);

	float3 sunDir = mainLight.direction;
	sunDir.x -= 0.05; //Fake a lower angle for effect to visible more often
	translucencyData = PopulateTranslucencyData(_ShallowColor.rgb * 20, sunDir, viewDirNorm, waveNormal, worldTangentNormal, waveHeight, transmissionMask, _TranslucencyParams);
	
	finalColor.rgb = ApplyTranslucency(finalColor.rgb, translucencyData);
#endif

	//Foam application
	#if _FOAM
	finalColor.rgb = lerp(finalColor.rgb, _FoamColor.rgb, foam);
	#endif

	#if _SHARP_INERSECTION || _SMOOTH_INTERSECTION
	//Layer intersection on top of everything
	finalColor.rgb = lerp(finalColor.rgb, _IntersectionColor.rgb, intersection);
	#endif

	alpha = saturate(alpha + intersection + foam);

	#if _FLAT_SHADING //Skip, not a good fit
	worldTangentNormal = waveNormal;
	#else
	//At this point, normal strength should affect lighting
	half normalMask = saturate((intersection + foam + (shoreSine * 2.0)));
	worldTangentNormal = lerp(waveNormal, worldTangentNormal, saturate(_NormalStrength - normalMask));
	#endif
	
	//return float4(normalMask, normalMask, normalMask, 1);

	//Horizon color (note: not using normals, since they are perturbed by waves)
	half VdotN = 1.0 - saturate(dot(viewDirNorm, normalWS));
	float fresnel = saturate(pow(VdotN, _HorizonDistance));
	#if UNDERWATER_ENABLED
	fresnel *= facing;
	#endif
	finalColor.rgb = lerp(finalColor.rgb, _HorizonColor.rgb, fresnel * _HorizonColor.a);

	//Final alpha
	float edgeFade = saturate(opaqueDist / (_EdgeFade * 0.01));
	alpha *= edgeFade;

	float4 diffuseProjector = SampleDiffuseProjectors(wPos);
	finalColor.rgb = lerp(finalColor.rgb, diffuseProjector.rgb, diffuseProjector.a);
	
	SurfaceData surf = (SurfaceData)0;

	surf.albedo = finalColor.rgb;
	surf.specular = 0;
	surf.metallic = 0;
	surf.smoothness = sunSpec.a;
	surf.normalTS = NormalsCombined;
	surf.emission = sunSpec.rgb;
	surf.occlusion = 1;
	surf.alpha = alpha;

	InputData inputData;
	inputData.positionWS = wPos;
	inputData.viewDirectionWS = viewDirNorm;
	inputData.shadowCoord = ShadowCoords;
	inputData.normalWS = worldTangentNormal;
	inputData.fogCoord = input.fogFactorAndVertexLight.x;

	inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
	inputData.bakedGI = SAMPLE_GI(input.lightmapUVOrVertexSH.xy, input.lightmapUVOrVertexSH.xyz, inputData.normalWS);

	float4 color = ApplyLighting(surf, inputData, translucencyData);

	#if UNDERWATER_ENABLED
	//TODO: Move to underwater rendering code so effects are in sync
	float verticalDist = saturate(distance(_WorldSpaceCameraPos.xz, wPos.xz) / 8);
	alpha *= lerp(verticalDist, 1, facing);	
	#endif
	
	#if _REFRACTION
		float3 refraction = SAMPLE_SCENE_COLOR(refractedScreenPos.xy / refractedScreenPos.w).rgb;
		//Chromatic
#if _ADVANCED_SHADING
		float red = SAMPLE_SCENE_COLOR(refractedScreenPos.xy / refractedScreenPos.w + float2((_ScreenParams.z - 1.0), 0)).r;
		float blue = SAMPLE_SCENE_COLOR(refractedScreenPos.xy / refractedScreenPos.w - float2((_ScreenParams.z - 1.0), 0)).b;
		refraction.r = red;
		refraction.b = blue;
#endif
		color.rgb = lerp(refraction, color.rgb, alpha);
		alpha = edgeFade;
	#endif

	color.a = alpha * saturate(alpha - vertexColor.g);
	ApplyFog(color.rgb, input.fogFactorAndVertexLight.x, ScreenPos, wPos);

	return color;
}