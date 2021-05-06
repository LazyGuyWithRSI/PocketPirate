using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GenericReference<T> : ScriptableObject
{
    public T Value;
}
