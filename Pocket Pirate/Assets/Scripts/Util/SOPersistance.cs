using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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

    //[SerializeField]
    //private Dictionary<string, SOContainer> SOContainerDict;

    public ReorderableList reorderableList;

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
        foreach (string key in data.FloatReferences.Keys)
        {
            //if (SOContainers.)
        }

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
