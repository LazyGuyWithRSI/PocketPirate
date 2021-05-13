using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTableReferenceData
{
    public Dictionary<int, WaveTableGroup[]> WaveTable;

    public List<WaveGroup> WaveGroups;

    public WaveTableReferenceData()
    {
        WaveTable = new Dictionary<int, WaveTableGroup[]>();
        WaveGroups = new List<WaveGroup>();
    }
}
