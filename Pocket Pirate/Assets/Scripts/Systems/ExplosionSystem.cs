using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplosionSystem : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public GameObject FlashPrefab;
    public GameObject SmallFlashPrefab;

    public void Start ()
    {
        // on what events do we want to spawn explosions?
        PubSub.RegisterListener<OnDeathEvent>(SpawnExplosion);
        PubSub.RegisterListener<OnShootEvent>(SpawnFlashOnShoot);
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
}
