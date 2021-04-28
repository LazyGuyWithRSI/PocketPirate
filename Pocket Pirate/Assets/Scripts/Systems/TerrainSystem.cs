using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSystem : MonoBehaviour
{
    public float noiseToUnitFactor = 10f;
    public float noiseScale = 1f;
    public float chanceToPlaceTerrain = 0.5f;
    public int width = 100;
    public int height = 100;

    public GameObject[] terrainPrefabs;

    private float[,] noiseMap;

    // Start is called before the first frame update
    void Start()
    {
        noiseMap = NoiseGenerator.Generate(width, height, noiseScale, Vector2.zero);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Random.Range(0f, 1f) < noiseMap[x, y] && Random.Range(0f, 1f) < chanceToPlaceTerrain)
                    GameObject.Instantiate(terrainPrefabs[Random.Range(0, terrainPrefabs.Length)], new Vector3((x - (width / 2)) * noiseToUnitFactor, 0, (y - (height / 2)) * noiseToUnitFactor), Quaternion.Euler(0, Random.Range(0f, 360f), 0));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
