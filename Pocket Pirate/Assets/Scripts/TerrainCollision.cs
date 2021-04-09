using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCollision : MonoBehaviour
{
    public float damage = 5.0f;
    public float damageCooldown = 2.0f;

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
        Debug.Log("Terrain Collision > Trigger enter. Other tag is " + other.tag);
        if (other.tag == "Terrain Trigger" && canBeDamaged)
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
