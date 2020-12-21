using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth = 10f;
    public int Team = 0;

    private float curHealth;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = MaxHealth;
    }

    public void TakeDamage(float amount)
    {
        // take arbitrary damage for now
        curHealth -= amount;
        if (curHealth <= 0)
        {
            //ExplosionSystem.Instance.SpawnExplosion(transform.position);
            PubSub.Publish<OnDeathEvent>(new OnDeathEvent() { Position = transform.position, Team = Team });
            GameObject.Destroy(gameObject);
        }
    }
}
