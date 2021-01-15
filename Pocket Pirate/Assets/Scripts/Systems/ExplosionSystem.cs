using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplosionSystem : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public GameObject FlashPrefab;
    public GameObject SmallFlashPrefab;

    public GameObject ExplosionDamagePrefab;

    public void Start ()
    {
        // on what events do we want to spawn explosions?
        PubSub.RegisterListener<OnDeathEvent>(SpawnExplosion); // use on explosion instead of on death
        PubSub.RegisterListener<OnShootEvent>(SpawnFlashOnShoot);
        PubSub.RegisterListener<DamagingExplosionEvent>(SpawnDamagingExplosion);
    }

    public void SpawnFlashOnShoot(object publishedEvent)
    {
        OnShootEvent args = publishedEvent as OnShootEvent;
        GameObject.Destroy(Instantiate(SmallFlashPrefab, args.Position, Quaternion.identity), 5);
    }

    public void SpawnExplosion(object publishedEvent)
    {
        OnDeathEvent args = publishedEvent as OnDeathEvent;
        args.Position = new Vector3(args.Position.x, args.Position.y + 1f, args.Position.z);
        GameObject.Destroy(Instantiate(ExplosionPrefab, args.Position, Quaternion.identity), 4);
        GameObject.Destroy(Instantiate(FlashPrefab, args.Position, Quaternion.identity), 5);
    }

    public void SpawnDamagingExplosion(object publishedEvent)
    {
        DamagingExplosionEvent args = publishedEvent as DamagingExplosionEvent;

        GameObject explosion = Instantiate(ExplosionDamagePrefab, args.Position, Quaternion.identity);
        explosion.GetComponent<DamagingExplosion>().Explode(10f, 0.1f, 15f);

        SpawnExplosion(new OnDeathEvent() { Position = args.Position, Team = 5});
    }
}
