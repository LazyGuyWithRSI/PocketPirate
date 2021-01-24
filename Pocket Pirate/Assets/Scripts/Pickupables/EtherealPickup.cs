using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtherealPickup : MonoBehaviour
{
    public float sinkTime = 10f;
    public float sinkTimeDeviation = 2f;

    private bool hasHitWater = false;

    // Start is called before the first frame update
    void Start ()
    {
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -0.5f, 0);
        StartCoroutine(SinkCoroutine());
    }

    private IEnumerator SinkCoroutine ()
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
        Ethereal otherEthereal = other.gameObject.GetComponent<Ethereal>();
        if (otherHealth != null && otherHealth.Team == 0 && otherEthereal != null) // player pickup
        {
            otherEthereal.ActivateEthereal();
            GameObject.Destroy(gameObject);
        }
    }
}
