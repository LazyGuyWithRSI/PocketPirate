using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCollision : MonoBehaviour
{
    public float damage = 5.0f;
    public float damageCooldown = 2.0f;
    public BoxCollider pickupCollider;
    public bool isActive = true;

    private bool canBeDamaged = true;

    private Health health;


    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Terrain Trigger" && canBeDamaged && isActive)
        {
            health.TakeDamage(damage);
            StartCoroutine("CooldownCoroutine");
        }
    }

    private IEnumerator CooldownCoroutine ()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(damageCooldown);
        canBeDamaged = true;
    }
}
