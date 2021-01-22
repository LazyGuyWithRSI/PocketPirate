using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int Worth = 100;
    public float sinkTime = 10f;
    public float sinkTimeDeviation = 2f;

    private bool hasHitWater = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SinkCoroutine());
    }

    private IEnumerator SinkCoroutine()
    {
        yield return new WaitForSeconds(sinkTime + Random.Range(-sinkTimeDeviation, sinkTimeDeviation));
        GetComponent<Buoyancy>().Sink();
        GameObject.Destroy(gameObject, 4f);
    }

    private void Update ()
    {
        if (!hasHitWater && transform.position.y <= 0.1)
        {
            PubSub.Publish<OnHitWater>(new OnHitWater() { Position = new Vector2(transform.position.x, transform.position.z) });
            hasHitWater = true;
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        Health otherHealth = other.gameObject.GetComponent<Health>();
        if (otherHealth != null && otherHealth.Team == 0) // player pickup
        {
            PubSub.Publish<OnCoinPickUpEvent>(new OnCoinPickUpEvent { Worth = Worth, Position = transform.position });
            GameObject.Destroy(gameObject);
        }
    }
}
