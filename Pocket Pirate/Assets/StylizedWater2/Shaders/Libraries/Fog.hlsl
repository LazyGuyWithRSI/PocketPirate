/* Configuration: UnityFog */

//Authors of third-party fog solutions can reach out to have their method integrated here

/* start UnityFog */
#define UnityFog
/* end UnityFog */

/* start Enviro */
//#define Enviro
/* end Enviro */

/* start Azure */
//#define Azure
/* end Azure */

/* start AtmosphericHeightFog */
//#define AtmosphericHeightFog
/* end AtmosphericHeightFog */

#ifdef Enviro
#include "Assets/Enviro - Sky and Weather/Core/Resources/Shaders/Core/EnviroFogCore.hlsl"
#endif

#ifdef Azure
#include "Assets/Azure[Sky] Dynamic Skybox/Shaders/Transparent/AzureFogCore.cginc"
#endif

#ifdef AtmosphericHeightFog
#include "Assets/BOXOPHOBIC/Atmospheric Height Fog/Core/Library/AtmosphericHeightFog.cginc"
#endif

//Executed in vertex stage
float CalculateFogFactor(float3 positionCS) {
	return ComputeFogFactor(positionCS.z);
}

//Fragment stage. Note: Screen position passed here is not normalized (divided by w-component)
void ApplyFog(inout float3 color, float fogFactor, float4 screenPos, float3 wPos) 
{
#ifdef UnityFog
	color = MixFog(color.rgb, fogFactor);
#endif
	
#ifdef Enviro
	color.rgb = TransparentFog(float4(color.rgb, 1), wPos, screenPos.xy / screenPos.w, fogFactor).rgb;
#endif
	
#ifdef Azure
	color.rgb = ApplyAzureFog(float4(color.rgb, 1), wPos).rgb;
#endif

#ifdef AtmosphericHeightFog
	float4 fogParams = GetAtmosphericHeightFog(wPos.xyz);
	color.rgb = ApplyAtmosphericHeightFog(color.rgb, fogParams);
#endif

}
