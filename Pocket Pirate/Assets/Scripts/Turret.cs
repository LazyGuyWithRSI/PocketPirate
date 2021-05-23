using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IShooter
{
    public float ShootCooldown = 5f;
    public float ShootInterval = 0.04f;
    public float ShootHeading = 0f;
    public float AimDeviation = 3f;
    public float RotationSpeed = 10f;
    public float AimHeading = 0f;

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

    void Update()
    {
        ShootHeading = -transform.localEulerAngles.y % 360;
        //if (ShootHeading < 0)
        //ShootHeading = 360 + ShootHeading;
        if (Mathf.Abs(AimHeading) > AimDeviation)
        {
            int direction = 1;
            if (AimHeading < 0)
                direction = -1;

            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + (RotationSpeed * direction * Time.deltaTime), 0);
        }

    }

    public bool Shoot()
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

    public void SetName(string Name)
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

    private IEnumerator CanFireCooldown(float duration)
    {
        canShoot = false;
        yield return new WaitForSeconds(duration);
        canShoot = true;

        if (!string.IsNullOrEmpty(Name))
        {
            PubSub.Publish<OnPlayerReloaded>(new OnPlayerReloaded() { WeaponName = Name });
        }
    }

    public void SetCooldown(float cooldown)
    {
        ShootCooldown = cooldown;
    }

    public float GetCooldown()
    {
        return ShootCooldown;
    }

    public void SetAimHeading(float heading)
    {
        AimHeading = heading;
    }
}
