using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadside : MonoBehaviour, IShooter
{
    public float ShootCooldown = 5f;
    public float ShootInterval = 0.04f;
    public float ShootHeading = 0f;

    [SerializeField] private UpgradablePropertyReference UpgradableReloadReference;

    private IShooter[] shooters;
    private bool canShoot = true;

    private string Name;

    void Start()
    {
        shooters = GetComponentsInChildren<IShooter>();

        if (UpgradableReloadReference != null)
        {
            ShootCooldown = UpgradableReloadReference.Value;
        }
    }

    public bool Shoot ()
    {
        if (!canShoot)
            return false;

        PubSub.Publish<OnShootEvent>(new OnShootEvent() { Position = transform.position });
        if (!string.IsNullOrEmpty(Name))
        {
            PubSub.Publish<OnPlayerFired>(new OnPlayerFired() { WeaponName = Name, ReloadDuration = ShootCooldown });
        }
        //           v start from 1 because GetComponentsInChildren includes self
        StartCoroutine(FireCoroutine(ShootInterval));

        StartCoroutine(CanFireCooldown(ShootCooldown));
        return true;
    }

    public void SetName (string Name)
    {
        this.Name = Name;
    }

    public bool CanShoot()
    {
        return canShoot;
    }

    public float GetShootHeading()
    {
        return ShootHeading;
    }

    private IEnumerator FireCoroutine(float interval)
    {
        for (int i = 1; i < shooters.Length; i++)
        {
            shooters[i].Shoot();
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator CanFireCooldown (float duration)
    {
        canShoot = false;
        yield return new WaitForSeconds(duration);
        canShoot = true;

        if (!string.IsNullOrEmpty(Name))
        {
            PubSub.Publish<OnPlayerReloaded>(new OnPlayerReloaded() { WeaponName = Name });
        }
    }

    public void SetCooldown (float cooldown)
    {
        ShootCooldown = cooldown;
    }

    public float GetCooldown ()
    {
        return ShootCooldown;
    }
}
