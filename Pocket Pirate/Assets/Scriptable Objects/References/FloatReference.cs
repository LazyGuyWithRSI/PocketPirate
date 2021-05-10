using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatReference : ScriptableObject, IResetable, IDataObjLoadable
{
    [SerializeField] private float baseValue = 1f;

    public float Value { get; set; }

    private void OnEnable()
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
