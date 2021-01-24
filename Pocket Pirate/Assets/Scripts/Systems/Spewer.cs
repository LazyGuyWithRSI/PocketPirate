using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spewer : MonoBehaviour
{
    public GameObject ThingToSpewPrefab;
    public Pickup[] Pickups;
    public float ChanceForPowerUp = 0.4f; // TODO use an SO (maybe change based on ship type? Or handle this elsewhere? (like a spawn powerup event?))

    public float SpewForce = 400f;

    [System.Serializable]
    public class Pickup
    {
        public GameObject Prefab;
        public float Weight = 1f;
    }

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
            float total = 0;
            foreach (Pickup pickup in Pickups)
            {
                total += pickup.Weight;
            }

            float rand = Random.Range(0f, total);
            for (int i = 0; i < Pickups.Length; i++)
            {
                if (rand < Pickups[i].Weight)
                {
                    SpawnThing(Pickups[i].Prefab, position, force);
                    break;
                }

                rand -= Pickups[i].Weight;
            }
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
