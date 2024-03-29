﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplosionSystem : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public GameObject BigExplosionPrefab;
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
        Vector3 newPos = args.Position + ((Camera.main.transform.position - args.Position).normalized * 2f);
        GameObject.Destroy(Instantiate(ExplosionPrefab, newPos, Quaternion.identity), 4);
        GameObject.Destroy(Instantiate(FlashPrefab, args.Position, Quaternion.identity), 5);
    }

    public void SpawnDamagingExplosion(object publishedEvent)
    {
        DamagingExplosionEvent args = publishedEvent as DamagingExplosionEvent;

        GameObject explosion = Instantiate(ExplosionDamagePrefab, args.Position, Quaternion.identity);
        explosion.GetComponent<DamagingExplosion>().Explode(args.Radius, 0.1f, args.Damage);

        args.Position = new Vector3(args.Position.x, args.Position.y + 1f, args.Position.z);
        Vector3 newPos = args.Position + ((Camera.main.transform.position - args.Position).normalized * 2f);
        GameObject.Destroy(Instantiate(BigExplosionPrefab, newPos, Quaternion.identity), 4);
        GameObject.Destroy(Instantiate(FlashPrefab, args.Position, Quaternion.identity), 5);

        CameraShake.Shake(0.15f, 0.5f);
    }
}
