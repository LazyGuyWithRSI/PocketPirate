using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth = 10f;
    public int Team = 0;

    private float curHealth;
    private bool dead;

    private Buoyancy buoyancy;
    private IBoatMover mover;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = MaxHealth;
        buoyancy = GetComponent<Buoyancy>();
        mover = GetComponent<IBoatMover>();
        dead = false;
    }

    public void TakeDamage(float amount)
    {
        // take arbitrary damage for now
        curHealth -= amount;
        if (curHealth <= 0 && !dead)
        {
            dead = true;
            //ExplosionSystem.Instance.SpawnExplosion(transform.position);
            PubSub.Publish<OnDeathEvent>(new OnDeathEvent() { Position = transform.position, Team = Team });

            if (buoyancy != null)
                buoyancy.Sink();

            if (mover != null)
                mover.SetMoving(0);

            GameObject.Destroy(gameObject, 6f);
        }
    }
}
