﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatReference : ScriptableObject
{
    [SerializeField] private float baseValue = 1f;

    public float Value { get; set; }

    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
        Value = baseValue;
    }
}