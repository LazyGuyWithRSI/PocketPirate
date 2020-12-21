using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemEvents : MonoBehaviour
{
    // events
    public static event Action<GameObject> ProjectileImpactedWater = delegate { };
    public static void InvokeProjectileImpactedWater(GameObject o)
    {
        ProjectileImpactedWater(o);
    }

    // Start is called before the first frame update
    void Start()
    {
        SystemEvents.ProjectileImpactedWater += handleWaterImpact;
    }

    public void handleWaterImpact(GameObject o)
    {
        GameObject.Destroy(o);
    }
}
