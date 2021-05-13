using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorRing : MonoBehaviour
{
    public Transform Target;
    public Transform Ring;
    public float HeightOffWater = 0.2f;
    public float XOffset = 0.0f;
    public float ZOffset = 0.0f;
    public float DistanceThreshold;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }

        // point towards nearest enemy
        Health closest = null;
        float closestDist = float.MaxValue;
        foreach (Health enemy in DebugEnemySpawner.Enemies)
        {
            if (enemy.curHealth <= 0)
                continue;

            float distanceSqr = (enemy.transform.position - transform.position).sqrMagnitude;
            if (distanceSqr < closestDist)
            {
                closestDist = distanceSqr;
                closest = enemy;
            }
        }

        if (closest != null && closestDist > DistanceThreshold)
        {
            Ring.gameObject.SetActive(true);
            Ring.LookAt(closest.transform);
            Ring.eulerAngles = new Vector3(0, Ring.eulerAngles.y, 0);
        }
        else
        {
            Ring.gameObject.SetActive(false);
        }

        transform.position = new Vector3(Target.position.x + XOffset, HeightOffWater, Target.position.z + ZOffset);
    }
}
