using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOPersistance : MonoBehaviour
{
    public static SOPersistance Instance;

    public TextAsset jsonFile;

    [System.Serializable]
    public class SOContainer
    {
        public string ID;
        public ScriptableObject Object;
    }

    // SO's to manipulate
    public SOContainer[] SOContainers;

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
        //JSONData serializedTest = JsonConvert.SerializeObject(JSONData);
        JSONData data = JsonConvert.DeserializeObject<JSONData>(jsonFile.text);
        //JSONData data = JsonUtility.FromJson<JSONData>(jsonFile.text);

        Debug.Log("data: " + data.ToString());

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
