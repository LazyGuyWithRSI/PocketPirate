using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float StrengthMultiplier = 100f;
    public float DepthForce = 0.5f;

    [System.Serializable]
    public class BuoyancyPoint
    {
        public Vector3 Position = Vector3.zero;
        public float Strength = 1f;
    }

    public BuoyancyPoint[] points;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    void FixedUpdate()
    {
        foreach (BuoyancyPoint point in points)
        {
            Vector3 worldPoint = transform.TransformPoint(point.Position);
            Vector3 force = Vector3.zero;
            if (worldPoint.y < 0)
                force = Vector3.up * point.Strength * StrengthMultiplier * ((Mathf.Abs(worldPoint.y) * DepthForce));

            rb.AddForceAtPosition(force * Time.fixedDeltaTime, worldPoint);
        }
    }

    private void OnDrawGizmosSelected ()
    {
        if (points == null)
            return;

        foreach (BuoyancyPoint point in points)
        {
            Gizmos.DrawWireSphere(transform.TransformPoint(point.Position), point.Strength / 10);
        }
    }
}


