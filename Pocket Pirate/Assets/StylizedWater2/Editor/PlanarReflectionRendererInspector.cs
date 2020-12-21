using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace StylizedWater2
{
    [CustomEditor(typeof(PlanarReflectionRenderer))]
    public class PlanarReflectionRendererInspector : Editor
    {
        private PlanarReflectionRenderer script;

        private void OnEnable()
        {
#if URP
            script = (PlanarReflectionRenderer)target;
            
            if (script.waterObjects.Count == 0 && WaterObject.Instances.Count == 1)
            {
                script.waterObjects.Add(WaterObject.Instances[0]);
                script.RecalculateBounds();
                script.EnableMaterialReflectionSampling();
                
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
#endif
        }

        public override void OnInspectorGUI()
        {
#if !URP
            UI.DrawNotification("Universal Render Pipeline isn't installed, or outdated", MessageType.Error);
#else
            UI.DrawNotification(XRSettings.enabled, "Not supported with VR rendering", MessageType.Error);

            base.OnInspectorGUI();
            
            if (script.waterObjects != null)
            {
                UI.DrawNotification(script.waterObjects.Count == 0, "Assign at least one WaterObject", MessageType.Info);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("Auto-find", EditorStyles.miniButton))
                {
                    script.waterObjects = new List<WaterObject>(WaterObject.Instances);
 
                    script.RecalculateBounds();
                    script.EnableMaterialReflectionSampling();
                    
                    EditorUtility.SetDirty(target);
                }
                if(GUILayout.Button("Recalculate bounds", EditorStyles.miniButton))
                {
                    script.RecalculateBounds();
                    
                    EditorUtility.SetDirty(target);
                }
            }
#endif
            
            UI.DrawFooter();
        }
    }
}
