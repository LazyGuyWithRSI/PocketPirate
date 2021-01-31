using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage = 5f;
    public int MaxHits = 1;
    public int Team = 0;

    private int hitsLeft;

    // Start is called before the first frame update
    void Start()
    {
        hitsLeft = MaxHits;
    }

    private void OnTriggerEnter (Collider other)
    {
        Health otherHealth = other.gameObject.GetComponent<Health>();
        if (otherHealth != null && otherHealth.Team != Team)
        {
            //if (otherHealth.TakeDamage(Damage))
            PubSub.Publish<OnHitEvent>(new OnHitEvent() { Position = transform.position, HitType = 0, Team = otherHealth.Team });
            otherHealth.TakeDamage(Damage);

            hitsLeft--;
            if (hitsLeft <= 0)
            {
                GameObject.Destroy(transform.parent.gameObject);
            }
        }
    }
}
