using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAvoidance : MonoBehaviour
{
    public float InfluenceScalar = 100f;
    public float AngleThreshold = 60f;
    IBoatMover mover;
    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<IBoatMover>();
    }

    // Update is called once per frame
    void Update()
    {
        Health closest = null;
        float closestDist = float.MaxValue;
        foreach (Health enemy in DebugEnemySpawner.Enemies)
        {
            if (enemy == null)
                continue;

            if (enemy.transform == transform) // self
                continue;

            float distanceSqr = (enemy.transform.position - transform.position).sqrMagnitude;
            if (distanceSqr < closestDist)
            {
                closestDist = distanceSqr;
                closest = enemy;
            }
        }

        if (closest != null)
        {
            float dir = AngleDir(transform.position, transform.forward, closest.transform.position);
            float realDistance = Vector3.Distance(transform.position, closest.transform.position);

            float influence = (1 / realDistance) * dir * InfluenceScalar;
            mover.SetTurnInfluence(influence);
            Debug.DrawRay(transform.position, transform.right * influence * 5f);
        }
        else
            mover.SetTurnInfluence(0);
    }

    public float AngleDir(Vector3 position, Vector3 forward, Vector3 target)
    {
        Vector3 vectorToTarget = target - position;
        float angle = Vector3.SignedAngle(vectorToTarget, forward, Vector3.up);

        if (Mathf.Abs(angle) > AngleThreshold)
            return 0;

        if (angle > 0)
            return 1;
        if (angle < 0)
            return -1;

        return 0;
    }

    public void OnDestroy()
    {
        DebugEnemySpawner.Enemies.Remove(GetComponent<Health>());
    }
}
