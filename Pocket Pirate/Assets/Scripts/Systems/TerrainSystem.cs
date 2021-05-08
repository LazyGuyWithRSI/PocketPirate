using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSystem : MonoBehaviour
{
    public float noiseToUnitFactor = 10f;
    public float noiseScale = 1f;
    public float chanceToPlaceTerrain = 0.5f;
    public float borderRadius = 50f;
    public float distanceBetweenBorderObjects = 5f;
    public int width = 100;
    public int height = 100;

    public GameObject[] terrainPrefabs;
    public Transform terrainParent;

    private float[,] noiseMap;

    // Start is called before the first frame update
    void Start()
    {
        // generate border

        // generate random terrain
        noiseMap = NoiseGenerator.Generate(width, height, noiseScale, Vector2.zero);
        float theta = 360 * (distanceBetweenBorderObjects / (2 * Mathf.PI * borderRadius));

        for (float a = 0; a < 360; a += theta)
        {
            GameObject newTerrain = GameObject.Instantiate(terrainPrefabs[1], new Vector3(Mathf.Sin(a) * borderRadius, 0, Mathf.Cos(a) * borderRadius), Quaternion.Euler(0, Random.Range(0f, 360f), 0));
            newTerrain.transform.parent = terrainParent;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // don't spawn anything in the middle
                if (Mathf.Abs((width / 2) - x) < 2 && Mathf.Abs((height / 2) - y) < 2)
                    continue;

                if (Random.Range(0f, 1f) < noiseMap[x, y] && Random.Range(0f, 1f) < chanceToPlaceTerrain)
                {
                    GameObject newTerrain = GameObject.Instantiate(terrainPrefabs[Random.Range(0, terrainPrefabs.Length)], new Vector3((x - (width / 2)) * noiseToUnitFactor, 0, (y - (height / 2)) * noiseToUnitFactor), Quaternion.Euler(0, Random.Range(0f, 360f), 0));
                    newTerrain.transform.parent = terrainParent;
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
