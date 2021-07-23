using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectOnSOCondition : MonoBehaviour
{
    public GameObject[] ObjectsToEnable;
    public ScriptableObject SO;
    public float Threshold;
    //public bool enableIfGreaterThanOrEqualTo;

    // Start is called before the first frame update
    void Start()
    {
        CheckValue();
        PubSub.RegisterListener<OnUpgradablePropertyChanged>(CheckValue);
    }

    private void CheckValue(object publishedEvent)
    {
        CheckValue();
    }

    private void CheckValue()
    {
        if (SO is UpgradablePropertyReference)
        {
            foreach (GameObject go in ObjectsToEnable)
            {
                go.SetActive((SO as UpgradablePropertyReference).Value >= Threshold);
            }
        }
    }
}
