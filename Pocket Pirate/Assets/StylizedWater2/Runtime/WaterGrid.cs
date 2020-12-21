using System;
using System.Collections;
using System.Collections.Generic;
using StylizedWater2;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace StylizedWater2
{
    [ExecuteInEditMode]
    [AddComponentMenu("Stylized Water 2/Water Grid")]
    public class WaterGrid : MonoBehaviour
    {
        [Tooltip("When not in play-mode, the water will follow the scene-view camera position.")]
        public bool followSceneCamera = false;

        [Tooltip("The water will follow this Transform's position on the XZ axis. Ideally set to the camera's transform.")]
        public Transform followTarget;
        
        [Header("Grid")]
        [Tooltip("Scale of the grid in the length and width")]
        public float scale = 500f;
        [Range(0.15f, 10f)] 
        [Tooltip("Distance between vertices, rather higher than lower")]
        public float vertexDistance = 2f;
        public int rowsColumns = 4;
        
        [Header("Appearance")]
        [Tooltip("Material used on the tile meshes")]
        public Material material;

        private float tileSize;
        private WaterMesh waterMesh;
        private Mesh mesh;
        [SerializeField]
        [HideInInspector]
        private List<WaterObject> objects = new List<WaterObject>();
        private Transform actualFollowTarget;
        private Vector3 targetPosition;

#if UNITY_EDITOR
        private void OnEnable()
        {
            UnityEditor.SceneView.duringSceneGui += OnSceneGUI;
            
            if(objects.Count == 0) Recreate();
        }
#endif

#if UNITY_EDITOR
        private void OnDisable()
        {
            UnityEditor.SceneView.duringSceneGui -= OnSceneGUI;
        }
#endif

        void Update()
        {
            if (Application.isPlaying) actualFollowTarget = followTarget;

            if (actualFollowTarget)
            {
                targetPosition = actualFollowTarget.transform.position;

                targetPosition = SnapToGrid(targetPosition, vertexDistance);
                targetPosition.y = this.transform.position.y;
                this.transform.position = targetPosition;
            }
        }

        public void Recreate()
        {
            if(waterMesh == null) waterMesh = new WaterMesh();
            waterMesh.shape = WaterMesh.Shape.Rectangle;
            waterMesh.size = Mathf.Max(10f, scale / rowsColumns);
            waterMesh.subdivisions = Mathf.FloorToInt(waterMesh.size / vertexDistance);
            waterMesh.UVTiling = waterMesh.size;
            mesh = waterMesh.Create();

            foreach (WaterObject obj in objects)
            {
                if (obj) DestroyImmediate(obj.gameObject);
            }
            objects.Clear();

            tileSize = mesh.bounds.size.x;

            rowsColumns = Mathf.Max(rowsColumns, 2);
            
            for (int x = 0; x < rowsColumns; x++)
            {
                for (int z = 0; z < rowsColumns; z++)
                {
                    WaterObject waterObject = WaterObject.New();
                    objects.Add(waterObject);

                    waterObject.transform.parent = this.transform;
                    waterObject.gameObject.layer = 4;

                    waterObject.name = "WaterTile_x" + x + "z" + z;
                    MeshRenderer r = waterObject.GetComponent<MeshRenderer>();
                    MeshFilter mf = waterObject.GetComponent<MeshFilter>();
                    mf.sharedMesh = mesh;
                    r.sharedMaterial = material;
                    waterObject.material = material;

                    waterObject.transform.localPosition = GridLocalCenterPosition(x, z);
                }
            }
        }

        private Vector3 GridLocalCenterPosition(int x, int z)
        {
            return new Vector3(x * tileSize - ((tileSize * (rowsColumns)) * 0.5f) + (tileSize * 0.5f), 0f,
                z * tileSize - ((tileSize * (rowsColumns)) * 0.5f) + (tileSize * 0.5f));
        }

        private static Vector3 SnapToGrid(Vector3 position, float cellSize)
        {
            return new Vector3(SnapToGrid(position.x, cellSize), SnapToGrid(position.y, cellSize), SnapToGrid(position.z, cellSize));
        }

        private static float SnapToGrid(float position, float cellSize)
        {
            return Mathf.FloorToInt(position / cellSize) * (cellSize) + (cellSize * 0.5f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.25f, 0.25f, 0.5f);
            
            for (int x = 0; x < rowsColumns; x++)
            {
                for (int z = 0; z < rowsColumns; z++)
                {
                    Vector3 pos = transform.TransformPoint(GridLocalCenterPosition(x, z));
                   
                    Gizmos.DrawWireCube(pos, new Vector3(tileSize, 0f, tileSize));
                }
            }
        }

#if UNITY_EDITOR
        private void OnSceneGUI(UnityEditor.SceneView sceneView)
        {
            if (followSceneCamera)
            {
                actualFollowTarget = sceneView.camera.transform;
                Update();
            }
            else
            {
                actualFollowTarget = null;
            }
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(WaterGrid))]
    public class CreateWaterGridInspector : Editor
    {
        private WaterGrid script;
        private int vertexCount;

        private void OnEnable()
        {
            script = (WaterGrid) target;
        }
        
        public override void OnInspectorGUI()
        {
            vertexCount = Mathf.FloorToInt(((script.scale / script.rowsColumns) / script.vertexDistance) * ((script.scale / script.rowsColumns) / script.vertexDistance));
            if(vertexCount > 65535)
            {
                EditorGUILayout.HelpBox("Vertex count of individual tiles is too high. Increase the vertex distance, decrease the grid scale, or add more rows/columns", MessageType.Error);
            }
            
            EditorGUI.BeginChangeCheck();
            
            base.OnInspectorGUI();
            
            if(script.material == null) EditorGUILayout.HelpBox("A material must be assigned", MessageType.Error);
            
            //Executed here since objects can't be destroyed from OnValidate
            if (EditorGUI.EndChangeCheck())
            {
                script.Recreate();
            }
        }
    }
#endif
}