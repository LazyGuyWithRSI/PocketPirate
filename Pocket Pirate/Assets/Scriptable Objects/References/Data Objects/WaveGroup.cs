using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WaveGroup
{
    public int ID;
    public string Name;
    public List<EnemySpawnData> EnemyData;
    public List<EnemySpawnGameObject> EnemyGameObjects;

    public WaveGroup()
    {
        EnemyData = new List<EnemySpawnData>();
        EnemyGameObjects = new List<EnemySpawnGameObject>();
    }
}

public struct EnemySpawnData
{
    public float Weight;
    public string EnemyName;
    public int SpawnLimit;
}

public struct EnemySpawnGameObject
{
    public float Weight;
    public GameObject GameObject;
    public int SpawnLimit;
}
