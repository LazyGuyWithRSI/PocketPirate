using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reference/Upgradable Property")]
public class UpgradablePropertyReference : ScriptableObject, IResetable, IDataObjLoadable
{
    public string DisplayName = "N/A";

    [SerializeField] private float baseValue = 1f;
    public float maxValue = 10f;
    public float stepSize = 1f;

    [SerializeField] private int baseCost = 100;
    public float costScalingFactor = 1.5f;

    [SerializeField] private bool Increasing = true;
    [SerializeField] private float RoundFactor = 50;

    public float Value { get; set; }
    public float Cost { get; set; }

    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
        Value = baseValue;
        Cost = baseCost;
    }

    public void Increment()
    {
        if (CanIncrement())
        {
            Value += stepSize;
            Cost = (int)Mathf.Round((Cost * costScalingFactor) / RoundFactor) * RoundFactor;
        }
    }

    public bool CanIncrement()
    {
        if (Increasing)
            return Value + stepSize <= maxValue;

        return Value + stepSize >= maxValue;
    }

    public float GetPercentageOfValue()
    {
        //if (Increasing)
            //return (Value / baseValue) * 100;

        return (Mathf.Abs(baseValue - Value) / baseValue) * 100;
    }

    public float GetPercentageOfStep()
    {
        return (Mathf.Abs(stepSize) / baseValue) * 100;
    }

    public void InitFromDataObj(object dataObj)
    {
        UpgradablePropertyReferenceData data = dataObj as UpgradablePropertyReferenceData;

        name = data.Name;

        baseValue = data.BaseValue;
        maxValue = data.MaxValue;
        stepSize = data.StepSize;

        baseCost = data.BaseCost;
        costScalingFactor = data.CostScalingFactor;
        Increasing = data.Increasing;
        RoundFactor = data.RoundFactor;

        Reset();
    }

    public void Reset()
    {
        Value = baseValue;
        Cost = baseCost;
    }
}
