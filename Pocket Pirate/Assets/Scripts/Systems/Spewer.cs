using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spewer : MonoBehaviour
{
    public GameObject ThingToSpewPrefab;
    public GameObject PowerUpPrefab; // TODO make generic system for (will power up spawn -> use weights to determine which one gets spawned)
    public float ChanceForPowerUp = 0.4f; // TODO use an SO (maybe change based on ship type? Or handle this elsewhere? (like a spawn powerup event?))

    public float SpewForce = 400f;

    // Start is called before the first frame update
    void Start()
    {
        PubSub.RegisterListener<OnSpewCoinsEvent>(OnSpewCoinsHandler);
    }

    public void OnSpewCoinsHandler (object publishedEvent) // TODO change to CoinSpew event or something
    {
        OnSpewCoinsEvent args = publishedEvent as OnSpewCoinsEvent;
        Spew(args.Position, 0, args.Amount, SpewForce);
    }

    public void Spew(Vector3 position, float duration, int amount, float force)
    {
        position.y += 0.3f;

        if (Random.Range(0f, 1f) <= ChanceForPowerUp)
        {
            SpawnThing(PowerUpPrefab, position, force);
        }

        for (int i = 0; i < amount; i++)
        {
            SpawnThing(ThingToSpewPrefab, position, force);
        }
    }

    private void SpawnThing(GameObject thingToSpew, Vector3 position, float force)
    {
        GameObject thing = Instantiate(thingToSpew, position, Quaternion.identity);
        Vector3 forceToApply = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 2f), Random.Range(-1f, 1f)) * force;
        Rigidbody rb = thing.GetComponent<Rigidbody>();
        rb.AddForce(forceToApply);
        rb.AddTorque(forceToApply);
    }

    private IEnumerator SpewCoroutine(Vector3 position, float duration, int amount, float force)
    {
        return null;
    }
}
