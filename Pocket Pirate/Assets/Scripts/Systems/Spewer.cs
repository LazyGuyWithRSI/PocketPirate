using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spewer : MonoBehaviour
{
    public GameObject ThingToSpewPrefab;

    // Start is called before the first frame update
    void Start()
    {
        PubSub.RegisterListener<OnSpewCoinsEvent>(OnSpewCoinsHandler);
    }

    public void OnSpewCoinsHandler (object publishedEvent) // TODO change to CoinSpew event or something
    {
        OnSpewCoinsEvent args = publishedEvent as OnSpewCoinsEvent;
        Spew(args.Position, 0, args.Amount, 500f);
    }

    public void Spew(Vector3 position, float duration, int amount, float force)
    {
        position.y += 0.3f;
        for (int i = 0; i < amount; i++)
        {
            GameObject thing = Instantiate(ThingToSpewPrefab, position, Quaternion.identity);
            Vector3 forceToApply = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 2f), Random.Range(-1f, 1f)) * force;
            Rigidbody rb = thing.GetComponent<Rigidbody>();
            rb.AddForce(forceToApply);
            rb.AddTorque(forceToApply);
        }
    }

    public IEnumerator SpewCoroutine(Vector3 position, float duration, int amount, float force)
    {
        return null;
    }
}
