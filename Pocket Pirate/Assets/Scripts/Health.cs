using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth = 10f;
    public int Team = 0;
    public int coinsOnDeath = 0;
    public float InvincibilityAfterDamageDuration = 0f;
    public GameObject ModelForFlash;
    public float FlashTime = 0.2f;

    [HideInInspector] public float curHealth;
    private bool dead;
    private bool canTakeDamage;

    private Buoyancy buoyancy;
    private IBoatMover mover;

    // TODO Getting overloaded, break these extra features out
    private MeshRenderer[] renderers;
    private Color[] originalColors;
    private bool canFlash;

    public bool ExplodeOnDeath = false;
    public float ExplosionDelay = 4.0f;
    public float ExplosionRadius = 10f;
    public float ExplosionDamage = 20f;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = MaxHealth;
        buoyancy = GetComponent<Buoyancy>();
        mover = GetComponent<IBoatMover>();
        dead = false;
        canTakeDamage = true;

        if (ModelForFlash != null)
        {
            renderers = ModelForFlash.GetComponentsInChildren<MeshRenderer>();
            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
                originalColors[i] = renderers[i].material.color;

            canFlash = true;
        }
        else
        {
            canFlash = false;
        }
    }

    public bool TakeDamage(float amount)
    {
        if (!canTakeDamage)
            return false;

        curHealth -= amount;

        // camera shake for player
        if (Team == 0)
        {
            CameraShake.Shake(0.2f, 0.2f);
            PubSub.Publish<OnHitEvent>(new OnHitEvent() { Position = transform.position, HitType = 0, Team = 0 });
        }

        if (canFlash)
            StartCoroutine(FlashCoroutine(FlashTime, Color.red));

        if (curHealth <= 0 && !dead)
        {
            dead = true;
            //ExplosionSystem.Instance.SpawnExplosion(transform.position);
            PubSub.Publish<OnDeathEvent>(new OnDeathEvent() { Position = transform.position, Team = Team });

            if (buoyancy != null)
                buoyancy.Sink();

            if (mover != null)
                mover.SetMoving(0);

            if (ExplodeOnDeath)
            {
                StartCoroutine(ExplodeOnDeathCoroutine(ExplosionDelay, 1f));
                GameObject.Destroy(gameObject, ExplosionDelay + 0.1f);
            }
            else
            {
                PubSub.Publish(new OnSpewCoinsEvent() { Amount = coinsOnDeath, Position = transform.position });
                GameObject.Destroy(gameObject, 6f);
            }
        }

        if (InvincibilityAfterDamageDuration != 0)
            StartCoroutine(InvincibleCooldown(InvincibilityAfterDamageDuration));

        return true;
    }

    public void heal(float amount)
    {
        curHealth = Mathf.Clamp(curHealth + amount, 0, MaxHealth);
        StartCoroutine(FlashCoroutine(FlashTime, Color.green));
        PubSub.Publish<OnPlayerHealed>(new OnPlayerHealed());
    }



    private IEnumerator ExplodeOnDeathCoroutine (float duration, float maxFlashTime)
    {
        float remainingDuration = duration;
        float currentFlashTime = 0f;
        while (remainingDuration > 0.4f)
        {
            currentFlashTime = Mathf.Min(remainingDuration / 4, maxFlashTime);
            StartCoroutine(FlashCoroutine(currentFlashTime / 2, Color.red));
            yield return new WaitForSeconds(currentFlashTime);
            remainingDuration -= currentFlashTime;
        }

        PubSub.Publish(new DamagingExplosionEvent() { Position = transform.position, Radius = ExplosionRadius, Damage = ExplosionDamage});
        PubSub.Publish(new OnSpewCoinsEvent() { Amount = coinsOnDeath, Position = transform.position });
    }

    private IEnumerator FlashCoroutine(float duration, Color color)
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = color;

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = originalColors[i];
    }

    private IEnumerator InvincibleCooldown (float duration)
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(duration);
        canTakeDamage = true;
    }
}
