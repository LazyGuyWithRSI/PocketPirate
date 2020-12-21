//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

//#define DEFAULT_GUI

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if URP
using UnityEditor.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#endif

namespace StylizedWater2
{
    public class MaterialUI : ShaderGUI
    {
#if URP
        private MaterialEditor materialEditor;

        Material targetMat;

        private MaterialProperty _ZWrite;
        private MaterialProperty _Cull;
        private MaterialProperty _ShadingMode;
        private MaterialProperty _AnimationParams;

        private MaterialProperty _BaseColor;
        private MaterialProperty _ShallowColor;
        private MaterialProperty _HorizonColor;
        private MaterialProperty _HorizonDistance;
        private MaterialProperty _Depth;
        private MaterialProperty _DepthExp;
        private MaterialProperty _WaveTint;
        private MaterialProperty _WorldSpaceUV;
        private MaterialProperty _TranslucencyParams;
        private MaterialProperty _EdgeFade;
        private MaterialProperty _ShadowStrength;

        private MaterialProperty _CausticsTex;
        private MaterialProperty _CausticsBrightness;
        private MaterialProperty _CausticsTiling;
        private MaterialProperty _CausticsDistortion;
        private MaterialProperty _RefractionStrength;

        private MaterialProperty _IntersectionSource;
        private MaterialProperty _IntersectionStyle;
        private MaterialProperty _IntersectionNoise;
        private MaterialProperty _IntersectionColor;
        private MaterialProperty _IntersectionLength;
        private MaterialProperty _IntersectionClipping;
        private MaterialProperty _IntersectionFalloff;
        private MaterialProperty _IntersectionTiling;
        private MaterialProperty _IntersectionRippleDist;
        private MaterialProperty _IntersectionRippleStrength;
        private MaterialProperty _IntersectionSpeed;

        private MaterialProperty _FoamTex;
        private MaterialProperty _FoamColor;
        private MaterialProperty _FoamSize;
        private MaterialProperty _FoamSpeed;
        private MaterialProperty _FoamTiling;
        private MaterialProperty _FoamWaveMask;
        private MaterialProperty _FoamWaveMaskExp;

        private MaterialProperty _BumpMap;
        private MaterialProperty _NormalTiling;
        private MaterialProperty _NormalStrength;
        private MaterialProperty _SparkleIntensity;
        private MaterialProperty _SparkleSize;

        private MaterialProperty _SunReflectionSize;
        private MaterialProperty _SunReflectionStrength;
        private MaterialProperty _SunReflectionDistortion;

        private MaterialProperty _ReflectionStrength;
        private MaterialProperty _ReflectionDistortion;
        private MaterialProperty _ReflectionBlur;
        private MaterialProperty _PlanarReflectionsParams;

        private MaterialProperty _WaveSpeed;
        private MaterialProperty _WaveHeight;
        private MaterialProperty _WaveNormalStr;
        private MaterialProperty _WaveDistance;
        private MaterialProperty _WaveSteepness;
        private MaterialProperty _WaveCount;
        private MaterialProperty _WaveDirection;

        //private MaterialProperty _ShoreLineWaveStr;
        //private MaterialProperty _ShoreLineWaveDistance;
        //private MaterialProperty _ShoreLineLength;

        private MaterialProperty _TessValue;
        private MaterialProperty _TessMin;
        private MaterialProperty _TessMax;

        private bool tesselationEnabled;

        private MaterialProperty _VertexColorMask;
        private MaterialProperty _DepthMode;

        //Keywords
        private bool enableFoam;
        private bool enableShadows;
        private bool enableEnvReflections;
        private bool enableSunReflection;
        private bool enableLighting;
        private bool enableTranslucency;
        private bool enableFlatShading;

        //Vectors
        private bool enableOpacityVC;
        private bool enableWaveVC;

        private UI.Material.Section generalSection;
        private UI.Material.Section lightingSection;
        private UI.Material.Section colorSection;
        private UI.Material.Section underwaterSection;
        private UI.Material.Section normalsSection;
        private UI.Material.Section reflectionSection;
        private UI.Material.Section intersectionSection;
        private UI.Material.Section foamSection;
        private UI.Material.Section wavesSection;
        private UI.Material.Section advancedSection;

        private MaterialProperty _DisableDepthTexture;
        private MaterialProperty _CausticsOn;
        private MaterialProperty _NormalMapOn;
        private MaterialProperty _RefractionOn;
        private MaterialProperty _WavesOn;

        private MaterialProperty _CurvedWorldBendSettings;
        
        private GUIContent simpleShadingContent;
        private GUIContent advancedShadingContent;

        private bool initliazed;
        private bool transparentShadowsEnabled;

        public void FindProperties(MaterialProperty[] props)
        {
            enableLighting = !targetMat.IsKeywordEnabled("_UNLIT");
            enableFoam = targetMat.IsKeywordEnabled("_FOAM");
            enableShadows = !targetMat.IsKeywordEnabled("_RECEIVE_SHADOWS_OFF");
            enableEnvReflections = !targetMat.IsKeywordEnabled("_ENVIRONMENTREFLECTIONS_OFF");
            enableSunReflection = !targetMat.IsKeywordEnabled("_SPECULARHIGHLIGHTS_OFF");
            transparentShadowsEnabled = StylizedWaterEditor.TransparentShadowsEnabled();
            tesselationEnabled = targetMat.HasProperty("_TessValue");
            enableTranslucency = targetMat.IsKeywordEnabled("_TRANSLUCENCY");
            enableFlatShading = targetMat.IsKeywordEnabled("_FLAT_SHADING");
            
            _IntersectionSource = FindProperty("_IntersectionSource", props);
            _IntersectionStyle = FindProperty("_IntersectionStyle", props);
            _VertexColorMask = FindProperty("_VertexColorMask", props);
            enableOpacityVC = _VertexColorMask.vectorValue.y == 1f;
            enableWaveVC = _VertexColorMask.vectorValue.z == 1f;

            if (tesselationEnabled)
            {
                _TessValue = FindProperty("_TessValue", props);
                _TessMin = FindProperty("_TessMin", props);
                _TessMax = FindProperty("_TessMax", props);
            }

            _Cull = FindProperty("_Cull", props);
            _ZWrite = FindProperty("_ZWrite", props);
            _ShadingMode = FindProperty("_ShadingMode", props);
            _ShadowStrength = FindProperty("_ShadowStrength", props);
            _AnimationParams = FindProperty("_AnimationParams", props);

            _DisableDepthTexture = FindProperty("_DisableDepthTexture", props);
            _RefractionOn = FindProperty("_RefractionOn", props);

            _BaseColor = FindProperty("_BaseColor", props);
            _ShallowColor = FindProperty("_ShallowColor", props);
            _HorizonColor = FindProperty("_HorizonColor", props);
            _HorizonDistance = FindProperty("_HorizonDistance", props);
            _Depth = FindProperty("_Depth", props);
            _DepthExp = FindProperty("_DepthExp", props);

            _WaveTint = FindProperty("_WaveTint", props);
            _WorldSpaceUV = FindProperty("_WorldSpaceUV", props);
            _TranslucencyParams = FindProperty("_TranslucencyParams", props);
            _EdgeFade = FindProperty("_EdgeFade", props);

            _CausticsOn = FindProperty("_CausticsOn", props);
            _CausticsTex = FindProperty("_CausticsTex", props);
            _CausticsBrightness = FindProperty("_CausticsBrightness", props);
            _CausticsTiling = FindProperty("_CausticsTiling", props);
            _CausticsDistortion = FindProperty("_CausticsDistortion", props);
            _RefractionStrength = FindProperty("_RefractionStrength", props);


            _IntersectionNoise = FindProperty("_IntersectionNoise", props);
            _IntersectionColor = FindProperty("_IntersectionColor", props);
            _IntersectionLength = FindProperty("_IntersectionLength", props);
            _IntersectionClipping = FindProperty("_IntersectionClipping", props);
            _IntersectionFalloff = FindProperty("_IntersectionFalloff", props);
            _IntersectionTiling = FindProperty("_IntersectionTiling", props);
            _IntersectionRippleDist = FindProperty("_IntersectionRippleDist", props);
            _IntersectionRippleStrength = FindProperty("_IntersectionRippleStrength", props);
            _IntersectionSpeed = FindProperty("_IntersectionSpeed", props);
            
            _FoamTex = FindProperty("_FoamTex", props);
            _FoamColor = FindProperty("_FoamColor", props);
            _FoamSize = FindProperty("_FoamSize", props);
            _FoamSpeed = FindProperty("_FoamSpeed", props);
            _FoamTiling = FindProperty("_FoamTiling", props);
            _FoamWaveMask = FindProperty("_FoamWaveMask", props);
            _FoamWaveMaskExp = FindProperty("_FoamWaveMaskExp", props);
            
            _NormalMapOn = FindProperty("_NormalMapOn", props);
            _BumpMap = FindProperty("_BumpMap", props);
            _NormalTiling = FindProperty("_NormalTiling", props);
            _NormalStrength = FindProperty("_NormalStrength", props);
            _SparkleIntensity = FindProperty("_SparkleIntensity", props);
            _SparkleSize = FindProperty("_SparkleSize", props);

            _SunReflectionSize = FindProperty("_SunReflectionSize", props);
            _SunReflectionStrength = FindProperty("_SunReflectionStrength", props);
            _SunReflectionDistortion = FindProperty("_SunReflectionDistortion", props);

            _ReflectionStrength = FindProperty("_ReflectionStrength", props);
            _ReflectionDistortion = FindProperty("_ReflectionDistortion", props);
            _ReflectionBlur = FindProperty("_ReflectionBlur", props);
            _PlanarReflectionsParams = FindProperty("_PlanarReflectionsParams", props);

            _WavesOn = FindProperty("_WavesOn", props);
            _WaveSpeed = FindProperty("_WaveSpeed", props);
            _WaveHeight = FindProperty("_WaveHeight", props);
            _WaveNormalStr = FindProperty("_WaveNormalStr", props);
            _WaveDistance = FindProperty("_WaveDistance", props);
            _WaveSteepness = FindProperty("_WaveSteepness", props);
            _WaveCount = FindProperty("_WaveCount", props);
            _WaveDirection = FindProperty("_WaveDirection", props);
            //_ShoreLineWaveStr = FindProperty("_ShoreLineWaveStr", props);
            //_ShoreLineWaveDistance = FindProperty("_ShoreLineWaveDistance", props);
            //_ShoreLineLength = FindProperty("_ShoreLineLength", props);

            //_DepthMode = FindProperty("_ShoreLineLength", props);
            
            if(targetMat.HasProperty("_CurvedWorldBendSettings")) _CurvedWorldBendSettings = FindProperty("_CurvedWorldBendSettings", props);
            
            simpleShadingContent = new GUIContent("Simple", 
             "Mobile friendly");

            advancedShadingContent = new GUIContent("Advanced",
                "Chromatic refraction\n" +
                "Point/spot light reflections & translucency\n" +
                "Caustics/sun reflection hidden in shadows");
        }

        private void OnEnable()
        {
            lightingSection = new UI.Material.Section(materialEditor,"LIGHTING", "Lighting");
            generalSection = new UI.Material.Section(materialEditor,"GENERAL", "General");
            colorSection = new UI.Material.Section(materialEditor,"COLOR", "Color");
            underwaterSection = new UI.Material.Section(materialEditor,"UNDERWATER", "Underwater");
            normalsSection = new UI.Material.Section(materialEditor,"NORMALS", "Normals");
            reflectionSection = new UI.Material.Section(materialEditor,"REFLECTIONS", "Reflections");
            foamSection = new UI.Material.Section(materialEditor,"FOAM", "Surface Foam");
            intersectionSection = new UI.Material.Section(materialEditor,"INTERSECTION", "Intersection Foam");
            wavesSection = new UI.Material.Section(materialEditor,"WAVES", "Waves");
            advancedSection = new UI.Material.Section(materialEditor,"ADVANCED", "Advanced");
            
            initliazed = true;
        }

        //https://github.com/Unity-Technologies/ScriptableRenderPipeline/blob/648184ec8405115e2fcf4ad3023d8b16a191c4c7/com.unity.render-pipelines.universal/Editor/ShaderGUI/BaseShaderGUI.cs
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            this.materialEditor = materialEditor;
            this.materialEditor.Repaint();

            materialEditor.SetDefaultGUIWidths();
            materialEditor.UseDefaultMargins();
            EditorGUIUtility.labelWidth = 0f;

            targetMat = materialEditor.target as Material;

            if (!initliazed) OnEnable();

            //Requires refetching for undo/redo to function
            FindProperties(props);

#if DEFAULT_GUI
            base.OnGUI(materialEditor, props);
            return;
#endif

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android || EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                if (PlayerSettings.GetUseDefaultGraphicsAPIs(EditorUserBuildSettings.activeBuildTarget) == false &&
                    PlayerSettings.GetGraphicsAPIs(EditorUserBuildSettings.activeBuildTarget)[0] == GraphicsDeviceType.OpenGLES2)
                {
                    UI.DrawNotification("You are targeting the OpenGLES 2.0 graphics API, which is not supported. Shader will not compile on the device", MessageType.Error);
                }
            }

            UI.DrawNotification(!UniversalRenderPipeline.asset, "No Universal Render Pipeline asset is currently in use", "Show me", StylizedWaterEditor.OpenGraphicsSettings ,MessageType.Error);
            
            if (UniversalRenderPipeline.asset)
            {
                UI.DrawNotification(
                    UniversalRenderPipeline.asset.supportsCameraDepthTexture == false &&
                    _DisableDepthTexture.floatValue == 0f,
                    "Depth texture is disabled, which is required for the material's current configuration",
                    "Enable",
                    StylizedWaterEditor.EnableDepthTexture,
                    MessageType.Error);
                
                UI.DrawNotification(
                    UniversalRenderPipeline.asset.supportsCameraOpaqueTexture == false && _RefractionOn.floatValue == 1f,
                    "Opaque texture is disabled, which is required for the material's current configuration",
                    "Enable",
                    StylizedWaterEditor.EnableOpaqueTexture,
                    MessageType.Error);
            }

            EditorGUI.BeginChangeCheck();

            DrawGeneral();
            DrawLighting();
            DrawColor();
            DrawNormals();
            DrawUnderwater();
            DrawFoam();
            DrawIntersection();
            DrawReflections();
            DrawWaves();
            DrawAdvanced();
            
            if (targetMat.HasProperty("_CurvedWorldBendSettings"))
            {
                EditorGUILayout.LabelField("Curved World 2020", EditorStyles.boldLabel);
                materialEditor.ShaderProperty(_CurvedWorldBendSettings, _CurvedWorldBendSettings.displayName);
                EditorGUILayout.Space();
            }

            if (EditorGUI.EndChangeCheck())
            {
                ApplyChanges();
            }

            UI.DrawFooter();
        }

        public override void OnMaterialPreviewGUI(MaterialEditor materialEditor, Rect rect, GUIStyle background)
        {
            UI.Material.DrawMaterialHeader(materialEditor, rect, background);
        }

        private void ApplyChanges()
        {
#if URP
            targetMat.SetTexture("_CausticsTex", _CausticsTex.textureValue);
            targetMat.SetTexture("_BumpMap", _BumpMap.textureValue);
            targetMat.SetTexture("_FoamTex", _FoamTex.textureValue);
            targetMat.SetTexture("_IntersectionNoise", _IntersectionNoise.textureValue);

            targetMat.SetVector("_VertexColorMask", new Vector4(_IntersectionSource.floatValue != 0 ? 1f : 0f, enableOpacityVC ? 1f : 0f, enableWaveVC ? 1f : 0f));

            //Keywords
            CoreUtils.SetKeyword(targetMat, "_NORMALMAP", targetMat.GetFloat("_NormalMapOn") == 1.0f);
            CoreUtils.SetKeyword(targetMat, "_UNLIT", !enableLighting);
            CoreUtils.SetKeyword(targetMat, "_FOAM", enableFoam);
            CoreUtils.SetKeyword(targetMat, "_ADVANCED_SHADING", targetMat.GetFloat("_ShadingMode") == 1f);
            CoreUtils.SetKeyword(targetMat, "_RECEIVE_SHADOWS_OFF", !enableShadows);
            CoreUtils.SetKeyword(targetMat, "_TRANSLUCENCY", enableTranslucency);
            CoreUtils.SetKeyword(targetMat, "_FLAT_SHADING", enableFlatShading);
            CoreUtils.SetKeyword(targetMat, "_ENVIRONMENTREFLECTIONS_OFF", !enableEnvReflections);
            CoreUtils.SetKeyword(targetMat, "_SPECULARHIGHLIGHTS_OFF", !enableSunReflection);
            CoreUtils.SetKeyword(targetMat, "_SHARP_INERSECTION", _IntersectionStyle.floatValue == 1);
            CoreUtils.SetKeyword(targetMat, "_SMOOTH_INTERSECTION", _IntersectionStyle.floatValue == 2);

            CoreUtils.SetKeyword(targetMat, "_DISABLE_DEPTH_TEX", _DisableDepthTexture.floatValue == 1.0f);
            if (targetMat.HasProperty("_RefractionOn")) CoreUtils.SetKeyword(targetMat, "_REFRACTION", targetMat.GetFloat("_RefractionOn") == 1.0f);

            EditorUtility.SetDirty(targetMat);
#endif
        }

        private void DrawGeneral()
        {
            generalSection.DrawHeader(() => SwitchSection(generalSection));
            
            if (EditorGUILayout.BeginFadeGroup(generalSection.anim.faded))
            {
                EditorGUILayout.Space();
                
                var cullingMode = (int)_Cull.floatValue;
                cullingMode = EditorGUILayout.Popup("Culling", cullingMode, new string[] { "Double-sided", "Front-faces", "Back-faces" });
                _Cull.floatValue = cullingMode;
                
                var uvMode = (int)_WorldSpaceUV.floatValue;
                uvMode = EditorGUILayout.Popup("UV Source", uvMode, new string[] { "Mesh UV", "World XZ projected" });
                _WorldSpaceUV.floatValue = uvMode;

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                float animSpeed = UI.Material.DrawFloatTicker(_AnimationParams.vectorValue.z, "Speed");
                
                Vector2 animDir = UI.Material.DrawVector2(_AnimationParams.vectorValue, "Direction");
                animDir.x = Mathf.Clamp(animDir.x, -1f, 1f);
                animDir.y = Mathf.Clamp(animDir.y, -1f, 1f);

                
                _AnimationParams.vectorValue = new Vector4(animDir.x, animDir.y, animSpeed, 0);
                
                EditorGUI.indentLevel--;
                
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawLighting()
        {
            lightingSection.DrawHeader(() => SwitchSection(lightingSection));

            if (EditorGUILayout.BeginFadeGroup(lightingSection.anim.faded))
            {
                EditorGUILayout.Space();

                enableLighting = UI.Material.Toggle(enableLighting, "Enable", "Color from lights and Ambient light will affect the material. Can be disabled if using Unlit shaders, or fixed lighting");
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Shading mode", GUILayout.Width(EditorGUIUtility.labelWidth));
                    _ShadingMode.floatValue = (float)GUILayout.Toolbar((int)_ShadingMode.floatValue,
                        new GUIContent[] { simpleShadingContent, advancedShadingContent }
                    );
                }

                if ((EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ||
                    EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS) && _ShadingMode.floatValue == 1f)
                {
                    UI.DrawNotification("The current shading mode is not intended to be used on mobile hardware", MessageType.Warning);
                }

                enableFlatShading = UI.Material.Toggle(enableFlatShading, "Flat shading", "When enabled, normals are calculated per mesh face, resulting in a faceted appearance (low poly look)");
                enableShadows = EditorGUILayout.Toggle("Receive shadows", enableShadows);

                if (enableShadows && !transparentShadowsEnabled && _ShadingMode.floatValue != 0)
                {
                    transparentShadowsEnabled = StylizedWaterEditor.TransparentShadowsEnabled();
                }
                UI.DrawNotification(enableShadows && !transparentShadowsEnabled,
                    "Transparent shadows are disabled in the default Forward renderer", "Show me",
                    StylizedWaterEditor.SelectForwardRenderer, MessageType.Warning);

                if (!enableLighting && enableShadows && transparentShadowsEnabled)
                {
                    UI.DrawNotification("Material cannot not receive shadows, because lighting is disabled", MessageType.Warning); ;
                }

                if (enableShadows)
                {
                    EditorGUI.indentLevel++;
                    UI.Material.DrawSlider(_ShadowStrength, "Strength");
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawColor()
        {
            colorSection.DrawHeader(() => SwitchSection(colorSection));

            if (EditorGUILayout.BeginFadeGroup(colorSection.anim.faded))
            {
                EditorGUILayout.Space();

                UI.Material.DrawColorField(_BaseColor, true, _BaseColor.displayName, "Base water color, alpha channel controls transparency");
                UI.Material.DrawColorField(_ShallowColor, true, _ShallowColor.displayName, "Water color in shallow areas, alpha channel controls transparency. Note that the caustics effect is visible here, setting the alpha to 100% hides caustics");
                using (new EditorGUI.DisabledGroupScope(_DisableDepthTexture.floatValue == 1f))
                {
                    UI.Material.DrawSlider(_Depth);
                    EditorGUI.indentLevel++;
                    UI.Material.Toggle(_DepthExp, label:"Exponential", tooltip:"Exponential depth works best for shallow water and relatively flat shores");
                    EditorGUI.indentLevel--;
                }
                
                EditorGUILayout.Space();

                enableOpacityVC = UI.Material.Toggle(enableOpacityVC, "Vertex color opacity", "The Green vertex color channel also adds opacity");
                using (new EditorGUI.DisabledGroupScope(_DisableDepthTexture.floatValue == 1f))
                {
                    UI.Material.DrawFloatField(_EdgeFade, "Edge fading", "Fades out the water where it intersects with opaque objects.\n\nRequires the depth texture option to be enabled");
                    _EdgeFade.floatValue = Mathf.Max(0f, _EdgeFade.floatValue);
                }
                EditorGUILayout.Space();

                UI.Material.DrawColorField(_HorizonColor, true, _HorizonColor.displayName, "Color as perceived on the horizon, where looking across the water");
                UI.Material.DrawSlider(_HorizonDistance);
                using (new EditorGUI.DisabledGroupScope(_WavesOn.floatValue == 0f))
                {
                    UI.Material.DrawSlider(_WaveTint, tooltip:"Adds a bright/dark tint based on wave height\n\nWaves feature must be enabled");
                }

                EditorGUILayout.Space();

                enableTranslucency = UI.Material.Toggle(enableTranslucency, "Translucency",
                    "Creates the appearance of sun light shining through the waves.\n\nNote that is only visible at grazing light angle");
                if (enableTranslucency)
                {
                    EditorGUI.indentLevel++;
                    float translucencyStr = EditorGUILayout.Slider("Strength", _TranslucencyParams.vectorValue.x , 0f, 1f);
                    float translucencyExp = EditorGUILayout.Slider("Exponent", _TranslucencyParams.vectorValue.y , 1f, 8f);

                    _TranslucencyParams.vectorValue = new Vector4(translucencyStr, translucencyExp, 0f, 0f);
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawNormals()
        {
            normalsSection.DrawHeader(() => SwitchSection(normalsSection));

            if (EditorGUILayout.BeginFadeGroup(normalsSection.anim.faded))
            {
                EditorGUILayout.Space();

                UI.Material.Toggle(_NormalMapOn, "Enable", "Normals add small-scale detail to the water surface, which in turn is used in various lighting techniques");

                if (_NormalMapOn.floatValue == 1f)
                {
                    materialEditor.TextureProperty(_BumpMap, "Normal map");
                    UI.Material.DrawFloatTicker(_NormalTiling, "Tiling");
                    UI.Material.DrawSlider(_NormalStrength, "Strength");
                    if (!enableLighting)
                    {
                        UI.DrawNotification("Lighting is disabled, normal strength has no effect", MessageType.Info);
                    }

                    EditorGUILayout.Space();
                    
                    EditorGUILayout.LabelField("Sparkles", EditorStyles.boldLabel);
                    UI.Material.DrawSlider(_SparkleIntensity, "Intensity");
                    UI.Material.DrawSlider(_SparkleSize, "Size");
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawUnderwater()
        {
            underwaterSection.DrawHeader(() => SwitchSection(underwaterSection));

            if (EditorGUILayout.BeginFadeGroup(underwaterSection.anim.faded))
            {
                EditorGUILayout.Space();

                materialEditor.ShaderProperty(_CausticsOn, "Caustics");
                
                if(_CausticsOn.floatValue == 1f && _ShallowColor.colorValue.a == 1f) UI.DrawNotification("Shallow water alpha is 100%, caustics are not visible in shallow water", MessageType.Warning);
                
                if (_CausticsOn.floatValue == 1)
                {
                    materialEditor.TextureProperty(_CausticsTex, "Texture (Additively blended)");
                    UI.Material.DrawFloatField(_CausticsBrightness);
                    UI.Material.DrawFloatTicker(_CausticsTiling);
                    UI.Material.DrawSlider(_CausticsDistortion, tooltip:"Note: Distortion strength is influenced by the strength of the normal map texture");
                }
                if (_DisableDepthTexture.floatValue == 1f && _CausticsOn.floatValue == 1f)
                {
                    UI.DrawNotification("Caustics are disabled because the \"Disable depth texture\" option is", MessageType.Error);
                }

               
                EditorGUILayout.Space();

                materialEditor.ShaderProperty(_RefractionOn, "Refraction");

                if (_RefractionOn.floatValue == 1f && _NormalMapOn.floatValue == 0f && _WavesOn.floatValue == 0f)
                {
                    UI.DrawNotification("Refraction will have no effect if normals and waves are both disabled", MessageType.Warning);
                }
                UI.Material.DrawSlider(_RefractionStrength, "Strength", tooltip:"Note: Distortion strength is influenced by the strength of the normal map texture");

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawFoam()
        {
            foamSection.DrawHeader(() => SwitchSection(foamSection));

            if (EditorGUILayout.BeginFadeGroup(foamSection.anim.faded))
            {
                EditorGUILayout.Space();

                enableFoam = UI.Material.Toggle(enableFoam, "Enable");

                if (enableFoam)
                {
                    materialEditor.TextureProperty(_FoamTex, "Texture (R=Mask)");
                    UI.Material.DrawColorField(_FoamColor, true);
                    UI.Material.DrawSlider(_FoamSize, tooltip:"Clips the texture based on grayscale values. This means if the foam texture is a hard black/white texture, it has no effect");
                    using (new EditorGUI.DisabledGroupScope(_WavesOn.floatValue == 0f))
                    {
                        UI.Material.DrawSlider(_FoamWaveMask, tooltip: "Opt to only show the foam on the highest points of waves");
                        EditorGUI.indentLevel++;
                        UI.Material.DrawSlider(_FoamWaveMaskExp, label:"Exponent", tooltip: "Pushes the mask more towards the top of the waves");
                        EditorGUI.indentLevel--;
                    }

                    UI.Material.DrawFloatTicker(_FoamTiling);
                    UI.Material.DrawFloatTicker(_FoamSpeed);
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawIntersection()
        {
            intersectionSection.DrawHeader(() => SwitchSection(intersectionSection));

            if (EditorGUILayout.BeginFadeGroup(intersectionSection.anim.faded))
            {
                EditorGUILayout.Space();
                
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Style", GUILayout.Width(EditorGUIUtility.labelWidth));
                    _IntersectionStyle.floatValue = GUILayout.Toolbar((int)_IntersectionStyle.floatValue,
                        new GUIContent[] { new GUIContent("None"), new GUIContent( "Sharp"), new GUIContent("Smooth"), }, GUILayout.MaxWidth((250f))
                    );
                }

                if (_IntersectionStyle.floatValue > 0)
                {
                    materialEditor.ShaderProperty(_IntersectionSource, new GUIContent("Gradient source", null, "The effect requires a grayscale gradient to work with, this sets what information should be used for this"));
                    if (_IntersectionSource.floatValue == 0 && _DisableDepthTexture.floatValue == 1f)
                    {
                        UI.DrawNotification("The depth texture option is disabled in the Advanced tab",
                            MessageType.Error);
                    }

                    materialEditor.TextureProperty(_IntersectionNoise, "Texture (R=Mask)");
                    UI.Material.DrawColorField(_IntersectionColor, true);
                    
                    UI.Material.DrawSlider(_IntersectionLength, tooltip:"Distance from objects/shore");
                    UI.Material.DrawSlider(_IntersectionFalloff, tooltip:"The falloff represents a gradient");
                    UI.Material.DrawFloatTicker(_IntersectionTiling);
                    UI.Material.DrawFloatTicker(_IntersectionSpeed, tooltip:"This value is multiplied by the Animation Speed value in the General tab");

                    UI.Material.DrawSlider(_IntersectionClipping, tooltip:"Clips the effect based on its underlying grayscale values.");
                    if (_IntersectionStyle.floatValue == 1f)
                    {
                        UI.Material.DrawFloatTicker(_IntersectionRippleDist, tooltip:"Distance between each ripples over the total intersection length");
                        UI.Material.DrawSlider(_IntersectionRippleStrength, tooltip:"Sets how much the ripples should be blended in with the effect");
                    }
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawReflections()
        {
            reflectionSection.DrawHeader(() => SwitchSection(reflectionSection));

            if (EditorGUILayout.BeginFadeGroup(reflectionSection.anim.faded))
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Sun", EditorStyles.boldLabel);
                enableSunReflection = EditorGUILayout.Toggle("Enable", enableSunReflection);
                if (enableSunReflection)
                {
                    UI.Material.DrawFloatField(_SunReflectionStrength);
                    UI.Material.DrawSlider(_SunReflectionSize);
                    UI.Material.DrawSlider(_SunReflectionDistortion, tooltip:"Note: Distortion is largely influenced by the strength of the normal map texture");
                }

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Skybox/Reflection probes", EditorStyles.boldLabel);
                enableEnvReflections = EditorGUILayout.Toggle(new GUIContent("Enable", null, "Enabled reflections from the skybox and reflection probes (Note that URP does not support probe blending yet)"), enableEnvReflections);

                UI.Material.DrawSlider(_PlanarReflectionsParams, tooltip:"Planar reflection becomes less pronounced at direct viewing angle with higher values");

                if (enableEnvReflections && RenderSettings.defaultReflectionMode == DefaultReflectionMode.Custom && RenderSettings.customReflection == null)
                {
                    UI.DrawNotification("Lighting settings: Environment reflections source is set to \"Custom\" without a cubemap assigned. No reflections will be visible", MessageType.Warning);
                }
                if (enableEnvReflections)
                {
                    if (Camera.current && Camera.current.orthographic)
                    {
                        UI.DrawNotification("Does not work with orthographic cameras, because URP doesn't support box projected reflections", MessageType.Warning);
                    }
                    UI.Material.DrawSlider(_ReflectionStrength);
                    UI.Material.DrawSlider(_ReflectionDistortion, tooltip:"Note: Distortion is largely influenced by the strength of the normal map texture");
                    UI.Material.DrawSlider(_ReflectionBlur);
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawWaves()
        {
            wavesSection.DrawHeader(() => SwitchSection(wavesSection));

            if (EditorGUILayout.BeginFadeGroup(wavesSection.anim.faded))
            {
                EditorGUILayout.Space();

                materialEditor.ShaderProperty(_WavesOn, "Enable");

                if (_WavesOn.floatValue == 1)
                {
                    UI.Material.DrawFloatTicker(_WaveSpeed, label:"Speed multiplier");
                    enableWaveVC = UI.Material.Toggle(enableWaveVC, "Vertex color flattening", "The Blue vertex color channel flattens waves\n\nNote: this does NOT affect buoyancy calculations!");
                    
                    UI.Material.DrawSlider(_WaveHeight, tooltip:"Waves will always push the water up from its base height, meaning waves never have a negative height");
                    UI.Material.DrawIntSlider(_WaveCount, tooltip:"Repeats the wave calculation X number of times, but with smaller waves each time");
                    Vector4 waveDir = _WaveDirection.vectorValue;
                    Vector2 waveDir1;
                    Vector2 waveDir2;
                    waveDir1.x = waveDir.x;
                    waveDir1.y = waveDir.y;
                    waveDir2.x = waveDir.z;
                    waveDir2.y = waveDir.w;

                    EditorGUILayout.LabelField("Direction");
                    EditorGUI.indentLevel++;
                    waveDir2 = EditorGUILayout.Vector2Field("Sub layer 2 (X)", waveDir2);
                    waveDir1 = EditorGUILayout.Vector2Field("Sub layer 1 (Z)", waveDir1);
                    EditorGUI.indentLevel--;

                    waveDir = new Vector4(waveDir1.x, waveDir1.y, waveDir2.x, waveDir2.y);
                    _WaveDirection.vectorValue = waveDir;
                    //_WaveDirection.vectorValue = EditorGUILayout.Vector4Field("Direction", _WaveDirection.vectorValue);
                    UI.Material.DrawSlider(_WaveDistance, tooltip:"Distance between waves");
                    UI.Material.DrawSlider(_WaveSteepness, tooltip:"Sharpness, depending on other settings here, a too high value will causes vertices to overlap");
                    UI.Material.DrawSlider(_WaveNormalStr, tooltip:"Normals affect how curved the surface is perceived for direct and ambient light. Without this, the water will appear flat");
                    
                    //EditorGUILayout.Space();
                    //UI.Material.DrawSlider(_ShoreLineWaveStr);
                    //UI.Material.DrawSlider(_ShoreLineWaveDistance);
                    //UI.Material.DrawSlider(_ShoreLineLength);
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawAdvanced()
        {
            advancedSection.DrawHeader(() => SwitchSection(advancedSection));

            if (EditorGUILayout.BeginFadeGroup(advancedSection.anim.faded))
            {
                EditorGUILayout.Space();

                UI.Material.Toggle(_ZWrite, "Depth writing (ZWrite)", "Enable to have the water perform transparency sorting on itself. Advisable with high waves.");

                UI.Material.Toggle(_DisableDepthTexture, "Disable depth texture", "Depth texture is used to measure the distance between the water surface and objects underneath it.\n\n" +
                                                                          "This is used for the color gradient and intersection effects");

                EditorGUILayout.Space();

                if (tesselationEnabled)
                {
                    EditorGUILayout.LabelField("Tesselation", EditorStyles.boldLabel);
                    UI.Material.DrawSlider(_TessValue);
                    UI.Material.DrawFloatField(_TessMin);
                    UI.Material.DrawFloatField(_TessMax);
                }

                EditorGUILayout.Space();

                materialEditor.EnableInstancingField();
                materialEditor.RenderQueueField();
                //materialEditor.DoubleSidedGIField();

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void SwitchSection(UI.Material.Section s)
        {
            generalSection.Expanded = (s == generalSection) && !generalSection.Expanded;
            lightingSection.Expanded = (s == lightingSection) && !lightingSection.Expanded;
            colorSection.Expanded = (s == colorSection) && !colorSection.Expanded;
            underwaterSection.Expanded = (s == underwaterSection) && !underwaterSection.Expanded;
            normalsSection.Expanded = (s == normalsSection) && !normalsSection.Expanded;
            reflectionSection.Expanded = (s == reflectionSection) && !reflectionSection.Expanded;
            intersectionSection.Expanded = (s == intersectionSection) && !intersectionSection.Expanded;
            foamSection.Expanded = (s == foamSection) && !foamSection.Expanded;
            wavesSection.Expanded = (s == wavesSection) && !wavesSection.Expanded;
            advancedSection.Expanded = (s == advancedSection) && !advancedSection.Expanded;
        }
#else
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            UI.DrawNotification("The Universal Render Pipeline v" + AssetInfo.MIN_URP_VERSION + " or newer is not installed", MessageType.Error);
        }
#endif
    }
}