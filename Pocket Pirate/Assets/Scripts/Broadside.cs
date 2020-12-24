using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadside : MonoBehaviour, IShooter
{
    public float ShootCooldown = 5f;

    private IShooter[] shooters;
    private bool canShoot = true;

    private string Name;

    public bool Shoot ()
    {
        if (!canShoot)
            return false;

        PubSub.Publish<OnShootEvent>(new OnShootEvent() { Position = transform.position });
        if (!string.IsNullOrEmpty(Name))
        {
            PubSub.Publish<OnPlayerFired>(new OnPlayerFired() { WeaponName = Name });
        }
        //           v start from 1 because GetComponentsInChildren includes self
        for (int i = 1; i < shooters.Length; i++)
            shooters[i].Shoot();

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

    void Start()
    {
        shooters = GetComponentsInChildren<IShooter>();
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
}
