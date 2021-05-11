using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SOContainer
{
    public string ID;
    public ScriptableObject Object;
}

public class SOPersistance : MonoBehaviour
{
    public static SOPersistance Instance;

    public TextAsset jsonFile;

    // SO's to manipulate
    public SOContainer[] SOContainers;

    public SOContainer[] EnemyPrefabContainers;

    public WaveTableReference WaveTableReference;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            LoadJSON();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void LoadJSON()
    {
        JSONData data = JsonConvert.DeserializeObject<JSONData>(jsonFile.text);

        // load up data to SO's
        foreach (SOContainer container in SOContainers)
        {
            if (data.FloatReferences.ContainsKey(container.ID))
                (container.Object as FloatReference).InitFromDataObj(data.FloatReferences[container.ID]);

            if (data.UpgradablePropertyReferences.ContainsKey(container.ID))
                (container.Object as UpgradablePropertyReference).InitFromDataObj(data.UpgradablePropertyReferences[container.ID]);
        }

        if (data.WaveTableReference != null)
            ParseWaveTable(data.WaveTableReference);
    }

    private void ParseWaveTable(WaveTableReferenceData waveTableData)
    {
        Dictionary<string, GameObject> enemyDict = new Dictionary<string, GameObject>();
        foreach (SOContainer enemyPrefabContainer in EnemyPrefabContainers)
        {
            enemyDict.Add(enemyPrefabContainer.ID, (enemyPrefabContainer.Object as GameObjectReference).Object);
        }

        int waveGroupIDCounter = 423;
        Dictionary<string, WaveGroup> WaveGroupDict = new Dictionary<string, WaveGroup>();
        foreach (WaveGroup waveGroup in waveTableData.WaveGroups)
        {
            waveGroup.ID = waveGroupIDCounter++;
            WaveGroupDict.Add(waveGroup.Name, waveGroup);

            foreach (EnemySpawnData enemyData in waveGroup.EnemyData)
            {
                if (enemyDict.ContainsKey(enemyData.EnemyName))
                    waveGroup.EnemyGameObjects.Add(new EnemySpawnGameObject { Weight = enemyData.Weight, GameObject = enemyDict[enemyData.EnemyName] });
            }
        }

        foreach (WaveTableGroup waveTableGroup in waveTableData.WaveTable.Values)
        {
            if (WaveGroupDict.ContainsKey(waveTableGroup.WaveGroupName))
                waveTableGroup.WaveGroupID = WaveGroupDict[waveTableGroup.WaveGroupName].ID;
        }

        WaveTableReference.InitFromDataObj(waveTableData);
        Debug.Log("Parsed wave table! Yay!");
    }

    public void ResetAll()
    {
        foreach (SOContainer so in SOContainers)
        {
            if (so.Object is IResetable)
                (so.Object as IResetable).Reset();
        }
    }

}
