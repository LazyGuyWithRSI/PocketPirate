using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashSystem : MonoBehaviour
{
    public GameObject SplashPrefab;

    public void Start ()
    {
        // on what events do we want to spawn explosions?
        PubSub.RegisterListener<OnHitWater>(HitWater);
        PubSub.RegisterListener<OnDeathEvent>(OnDeath);
    }

    public void HitWater (object publishedEvent)
    {
        OnHitWater args = publishedEvent as OnHitWater;
        spawnSplash(args.Position);
    }

    public void OnDeath (object publishedEvent)
    {
        OnDeathEvent args = publishedEvent as OnDeathEvent;
        spawnSplash(new Vector2(args.Position.x, args.Position.z));
    }

    private void spawnSplash(Vector2 pos)
    {
        GameObject.Destroy(Instantiate(SplashPrefab, new Vector3(pos.x, 0.5f, pos.y), Quaternion.identity), 5);
    }
}
