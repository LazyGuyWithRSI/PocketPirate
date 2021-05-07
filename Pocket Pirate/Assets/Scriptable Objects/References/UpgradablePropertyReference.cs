using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reference/Upgradable Property")]
public class UpgradablePropertyReference : ScriptableObject
{
    public string Name = "N/A";

    [SerializeField] private float baseValue = 1f;
    public float maxValue = 10f;
    public float stepSize = 1f;

    [SerializeField] private int baseCost = 100;
    public float costScalingFactor = 1.5f;

    [SerializeField] private bool Increasing = true;

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
            Cost = (int)(Cost * costScalingFactor);
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
}
