using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reference/Float")]
public class FloatReference : ScriptableObject, IResetable, IDataObjLoadable
{
    [SerializeField] private float baseValue = 1f;

    public float Value { get; set; }

    public void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
        Value = baseValue;
    }

    public void InitFromDataObj(object dataObj)
    {
        FloatReferenceData data = dataObj as FloatReferenceData;
        baseValue = data.BaseValue;
        Reset();
    }

    public void Reset()
    {
        Value = baseValue;
    }
}
