using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reference/SpawningWave")]
public class SpawningWaveReference : ScriptableObject
{
    public ThingToSpawn[] ThingsToSpawn;

    [System.Serializable]
    public class ThingToSpawn
    {
        public GameObject Prefab;
        public float Weight = 1.0f;
    }
}
