using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reference/Wave Table")]
public class WaveTableReference : ScriptableObject, IResetable, IDataObjLoadable
{
    public Dictionary<int, WaveTableGroup> WaveTable;
    public List<WaveGroup> CurrentWavePool;

    public List<WaveGroup> WaveGroups;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
