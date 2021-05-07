using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOPersistance : MonoBehaviour
{
    public static SOPersistance Instance;

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
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
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
