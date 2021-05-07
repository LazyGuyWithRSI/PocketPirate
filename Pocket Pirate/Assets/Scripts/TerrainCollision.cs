using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCollision : MonoBehaviour
{
    public float damage = 5.0f;
    public float damageCooldown = 2.0f;
    public float BounceForce = 50f;
    public BoxCollider pickupCollider;
    public bool isActive = true;

    private bool canBeDamaged = true;

    private Health health;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.transform.tag == "Terrain Trigger" && isActive)
        {

            Vector3 force = collision.impulse.normalized;
            Vector3 point = collision.GetContact(0).point;
            force.y = 0;
            point.y = transform.position.y;
            //rb.AddForce(force * BounceForce);
            Vector3 localPoint = transform.InverseTransformPoint(point);
            localPoint.x = Mathf.Max(3f, localPoint.x);
            point = transform.TransformPoint(localPoint);
            rb.AddForceAtPosition(force * BounceForce, point, ForceMode.Acceleration);

            CameraShake.Shake(0.3f, 0.3f);

            if (canBeDamaged)
            {
                health.TakeDamage(damage);
                StartCoroutine("CooldownCoroutine");
            }
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        /*
        if (other.tag == "Terrain Trigger" && canBeDamaged && isActive)
        {
            health.TakeDamage(damage);
            StartCoroutine("CooldownCoroutine");
        }*/
    }

    private IEnumerator CooldownCoroutine ()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(damageCooldown);
        canBeDamaged = true;
    }
}
