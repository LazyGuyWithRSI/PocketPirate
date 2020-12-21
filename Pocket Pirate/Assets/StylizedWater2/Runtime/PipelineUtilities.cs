//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#if URP
using UnityEngine.Rendering.Universal;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StylizedWater2
{
    public static class PipelineUtilities
    {
        private const string renderDataListFieldName = "m_RendererDataList";
        
#if URP
        /// <summary>
        /// Retrieves a ForwardRenderer asset in the project, based on GUID
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static ForwardRendererData GetRenderer(string GUID)
        {
#if UNITY_EDITOR
            string assetPath = AssetDatabase.GUIDToAssetPath(GUID);
            ForwardRendererData renderer = (ForwardRendererData)AssetDatabase.LoadAssetAtPath(assetPath, typeof(ForwardRendererData));

            return renderer;
#else
            Debug.LogError("PipelineUtilities.GetRenderer() cannot be called in a build, it requires AssetDatabase. References to renderers should be saved beforehand!");
            return null;
#endif
        }
        
        /// <summary>
        /// Checks if a ForwardRenderer has been assigned to the pipeline asset
        /// </summary>
        /// <param name="renderer"></param>
        public static bool IsRendererAdded(ScriptableRendererData renderer)
        {
            if (renderer == null)
            {
                Debug.LogError("Pass is null");
                return false;
            }

            if (UniversalRenderPipeline.asset)
            {
                BindingFlags bindings =
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

                ScriptableRendererData[] m_rendererDataList =
                    (ScriptableRendererData[]) typeof(UniversalRenderPipelineAsset)
                        .GetField(renderDataListFieldName, bindings).GetValue(UniversalRenderPipeline.asset);
                bool isPresent = false;

                for (int i = 0; i < m_rendererDataList.Length; i++)
                {
                    if (m_rendererDataList[i] == renderer) isPresent = true;
                }

                return isPresent;
            }
            else
            {
                Debug.LogError("No Universal Render Pipeline is currently active.");
                return false;
            }
        }
        
        
        /// <summary>
        /// Adds a ForwardRenderer to the pipeline asset in use
        /// </summary>
        /// <param name="renderer"></param>
        private static void AddRendererToPipeline(ScriptableRendererData renderer)
        {
            if (renderer == null) return;

            if (UniversalRenderPipeline.asset)
            {
                BindingFlags bindings = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

                ScriptableRendererData[] m_rendererDataList = (ScriptableRendererData[]) typeof(UniversalRenderPipelineAsset).GetField(renderDataListFieldName, bindings).GetValue(UniversalRenderPipeline.asset);
                List<ScriptableRendererData> rendererDataList = new List<ScriptableRendererData>();

                for (int i = 0; i < m_rendererDataList.Length; i++)
                {
                    rendererDataList.Add(m_rendererDataList[i]);
                }

                rendererDataList.Add(renderer);

                typeof(UniversalRenderPipelineAsset).GetField(renderDataListFieldName, bindings).SetValue(UniversalRenderPipeline.asset, rendererDataList.ToArray());

#if UNITY_EDITOR
                EditorUtility.SetDirty(UniversalRenderPipeline.asset);
#endif
            }
            else
            {
                Debug.LogError("No Universal Render Pipeline is currently active.");
            }
        }

        /// <summary>
        /// Gets the renderer from the current pipeline asset that's marked as default
        /// </summary>
        /// <returns></returns>
        public static ScriptableRendererData GetDefaultRenderer()
        {
            if (UniversalRenderPipeline.asset)
            {
                ScriptableRendererData[] rendererDataList = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset)
                        .GetField(renderDataListFieldName, BindingFlags.NonPublic | BindingFlags.Instance)
                        .GetValue(UniversalRenderPipeline.asset);
                int defaultRendererIndex = (int)typeof(UniversalRenderPipelineAsset)
                    .GetField("m_DefaultRendererIndex", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(UniversalRenderPipeline.asset);

                return rendererDataList[defaultRendererIndex];
            }
            else
            {
                Debug.LogError("No Universal Render Pipeline is currently active.");
                return null;
            }
        }

        /// <summary>
        /// Checks if a ScriptableRendererFeature is added to the default renderer
        /// </summary>
        /// <param name="addIfMissing"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool RenderFeatureAdded<T>(bool addIfMissing = false)
        {
            ScriptableRendererData forwardRenderer = GetDefaultRenderer();
            bool isPresent = false;

            foreach (ScriptableRendererFeature feature in forwardRenderer.rendererFeatures)
            {
                if (feature.GetType() == typeof(T)) isPresent = true;
            }
            
            if(!isPresent && addIfMissing) AddRenderFeature<T>(forwardRenderer);
            
            return isPresent;
        }

        /// <summary>
        /// Adds a ScriptableRendererFeature to the renderer (default is none is supplied)
        /// </summary>
        /// <param name="forwardRenderer"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddRenderFeature<T>(ScriptableRendererData forwardRenderer = null)
        {
            if (forwardRenderer == null) forwardRenderer = GetDefaultRenderer();
            
            ScriptableRendererFeature feature = (ScriptableRendererFeature)ScriptableRendererFeature.CreateInstance(typeof(T).ToString());
            feature.name = typeof(T).ToString();
            
            //Add component https://github.com/Unity-Technologies/Graphics/blob/d0473769091ff202422ad13b7b764c7b6a7ef0be/com.unity.render-pipelines.universal/Editor/ScriptableRendererDataEditor.cs#L180
#if UNITY_EDITOR
            AssetDatabase.AddObjectToAsset(feature, forwardRenderer);
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(feature, out var guid, out long localId);
#endif

            //Get feature list
            FieldInfo renderFeaturesInfo = typeof(ScriptableRendererData).GetField("m_RendererFeatures", BindingFlags.Instance | BindingFlags.NonPublic);
            List<ScriptableRendererFeature> m_RendererFeatures = (List<ScriptableRendererFeature>)renderFeaturesInfo.GetValue(forwardRenderer);

            //Modify and set list
            m_RendererFeatures.Add(feature);
            renderFeaturesInfo.SetValue(forwardRenderer, m_RendererFeatures);

            //Onvalidate will call ValidateRendererFeatures and update m_RendererPassMap
            MethodInfo validateInfo = typeof(ScriptableRendererData).GetMethod("OnValidate", BindingFlags.Instance | BindingFlags.NonPublic);
            validateInfo.Invoke(forwardRenderer, null);

#if UNITY_EDITOR
            EditorUtility.SetDirty(forwardRenderer);
            AssetDatabase.SaveAssets();
#endif
            
            Debug.Log("<b>" + feature.name + "</b> was added to the " + forwardRenderer.name + " renderer");
        }

        public static void RemoveRendererFromPipeline(ScriptableRendererData renderer)
        {
            if (renderer == null) return;

            if (UniversalRenderPipeline.asset)
            {
                BindingFlags bindings =
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

                ScriptableRendererData[] m_rendererDataList =
                    (ScriptableRendererData[]) typeof(UniversalRenderPipelineAsset)
                        .GetField(renderDataListFieldName, bindings).GetValue(UniversalRenderPipeline.asset);
                List<ScriptableRendererData> rendererDataList = new List<ScriptableRendererData>(m_rendererDataList);

                if (rendererDataList.Contains(renderer)) rendererDataList.Remove((renderer));

                typeof(UniversalRenderPipelineAsset).GetField(renderDataListFieldName, bindings)
                    .SetValue(UniversalRenderPipeline.asset, rendererDataList.ToArray());

#if UNITY_EDITOR
                EditorUtility.SetDirty(UniversalRenderPipeline.asset);
                AssetDatabase.SaveAssets();
#endif
            }
            else
            {
                Debug.LogError("No Universal Render Pipeline is currently active.");
            }
        }

        /// <summary>
        /// Sets the renderer index of the related forward renderer
        /// </summary>
        /// <param name="camData"></param>
        /// <param name="renderer"></param>
        public static void AssignRendererToCamera(UniversalAdditionalCameraData camData, ScriptableRendererData renderer)
        {
            if (UniversalRenderPipeline.asset)
            {
                if (renderer)
                {
                    //list is internal, so perform reflection workaround
                    ScriptableRendererData[] rendererDataList = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset).GetField(renderDataListFieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(UniversalRenderPipeline.asset);

                    for (int i = 0; i < rendererDataList.Length; i++)
                    {
                        if (rendererDataList[i] == renderer) camData.SetRenderer(i);
                    }
                }
            }
            else
            {
                Debug.LogError("No Universal Render Pipeline is currently active.");
            }
        }
#endif
    }
}
