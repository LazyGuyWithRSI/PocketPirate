using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float StrengthMultiplier = 100f;
    public float DepthForce = 0.5f;

    public float SinkSpeed = 0.005f;

    private BoxCollider projectileTriggerCollider; // TODO COUPLING

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
        projectileTriggerCollider = GetComponent<BoxCollider>();
    }

    public void Sink()
    {
        if (projectileTriggerCollider != null)
            projectileTriggerCollider.enabled = false;

        // pick 2 random points and reduce their buoyancy
        if (points.Length < 2)
            points[0].Strength = 0;
        else
        {
            StartCoroutine(SinkPointRoutine(UnityEngine.Random.Range(0, points.Length - 1), 0.4f, 2f));
            //points[UnityEngine.Random.Range(0, points.Length - 1)].Strength *= 0.2f;
            //points[UnityEngine.Random.Range(0, points.Length - 1)].Strength *= 0.2f;
            StartCoroutine(SinkRoutine(5f));
        }    
    }

    private IEnumerator SinkPointRoutine(int index, float rate, float duration)
    {
        float multiplier = 1.0f;
        float time = 0;
        float originalStrength = points[index].Strength;
        while (time <= duration)
        {
            multiplier -= (rate * Time.deltaTime);
            points[index].Strength = originalStrength * multiplier;
            time += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator SinkRoutine (float duration)
    {
        float multiplier = 1.0f;
        float time = 0;
        while (time <= duration)
        {
            multiplier -= (SinkSpeed * Time.deltaTime);
            StrengthMultiplier *= multiplier;
            time += Time.deltaTime;
            yield return null;
        }
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


