using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reference/Wave Table")]
public class WaveTableReference : ScriptableObject, IResetable, IDataObjLoadable
{
    public Dictionary<int, WaveTableGroup[]> WaveTable;
    public List<WaveGroup> CurrentWavePool;

    public List<WaveGroup> WaveGroups;

    private int lastWave = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateWavePool(int waveNumber)
    {
        if (lastWave == waveNumber)
            return;

        for (int i = lastWave; i <= waveNumber; i++)
        {
            Debug.Log("Adding wave " + i);
            if (WaveTable.ContainsKey(i))
            {
                foreach (WaveTableGroup tableGroup in WaveTable[i])
                {
                    if (tableGroup.ClearPool)
                        CurrentWavePool.Clear();

                    if (tableGroup.IsAdding)
                        CurrentWavePool.Add(WaveGroups.Find(x => x.ID == tableGroup.WaveGroupID));
                    else
                        CurrentWavePool.Remove(WaveGroups.Find(x => x.ID == tableGroup.WaveGroupID));

                    Debug.Log("Wave pool size: " + CurrentWavePool.Count);
                }
            }
        }

        lastWave = waveNumber;
    }

    public void InitFromDataObj(object dataObj)
    {
        WaveTableReferenceData data = dataObj as WaveTableReferenceData;
        WaveTable = data.WaveTable;
        WaveGroups = data.WaveGroups;

        CurrentWavePool = new List<WaveGroup>();
    }

    public void Reset()
    {
        CurrentWavePool = new List<WaveGroup>();
        UpdateWavePool(1);
    }
}
