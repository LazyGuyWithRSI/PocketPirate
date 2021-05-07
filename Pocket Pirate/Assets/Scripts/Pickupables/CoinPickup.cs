using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int Worth = 100;
    public float sinkTime = 10f;
    public float sinkTimeDeviation = 2f;
    public float EndOfWaveAttractionRadius = 10f;
    public SphereCollider AttractionCollider;

    private bool hasHitWater = false;
    private bool gameIsOver = false;

    // Start is called before the first frame update
    void Start()
    {
        PubSub.RegisterListener<OnGameOver>(OnGameOverHandler);
        PubSub.RegisterListener<OnWaveOver>(OnWaveOverHandler);

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
        if (!other.CompareTag("Pickup Collider") || gameIsOver)
            return;

        Health otherHealth = other.gameObject.transform.parent.GetComponent<Health>();
        if (otherHealth != null && otherHealth.Team == 0) // player pickup
        {
            PubSub.Publish<OnCoinPickUpEvent>(new OnCoinPickUpEvent { Worth = Worth, Position = transform.position });
            GameObject.Destroy(gameObject);
        }
    }

    public void OnGameOverHandler (object publishedEvent)
    {
        gameIsOver = true;
    }

    public void OnWaveOverHandler (object publishedEvent)
    {
        WaveOver();
    }

    public void WaveOver()
    {
        if (AttractionCollider != null)
            AttractionCollider.radius = EndOfWaveAttractionRadius;
    }
}
