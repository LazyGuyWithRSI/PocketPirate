using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingExplosion : MonoBehaviour
{
    public float Damage = 20f;
    public int Team = 0;

    private SphereCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<SphereCollider>();
    }
    public void Explode(float Radius, float duration, float Damage)
    {
        collider = GetComponent<SphereCollider>();
        this.Damage = Damage;
        StartCoroutine(ExplodeCoroutine(duration, Radius));
    }

    private IEnumerator ExplodeCoroutine(float duration, float radius)
    {
        collider.radius = radius;
        yield return new WaitForSeconds(duration);
        collider.radius = 0;
        GameObject.Destroy(gameObject);
    }

    private void OnTriggerEnter (Collider other)
    {
        Health otherHealth = other.gameObject.GetComponent<Health>();
        if (otherHealth != null && otherHealth.Team != Team)
        {
            //if (otherHealth.TakeDamage(Damage))
            PubSub.Publish<OnHitEvent>(new OnHitEvent() { Position = transform.position, HitType = 0, Team = otherHealth.Team });
            otherHealth.TakeDamage(Damage);
        }
    }
}
