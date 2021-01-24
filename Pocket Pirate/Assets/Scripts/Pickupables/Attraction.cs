using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour
{
    public float ForceMultiplier = 100f;
    Transform target;
    bool inRange = false;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start ()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (inRange)
        {
            Vector3 force = target.position - transform.position;
            Debug.Log("Applying " + (force.normalized * ForceMultiplier * Time.fixedDeltaTime));
            rb.AddForce(force * ForceMultiplier * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        Health otherHealth = other.gameObject.GetComponent<Health>();
        if (otherHealth != null && otherHealth.Team == 0) // player
        {
            Debug.Log("triggering");
            target = other.transform;
            inRange = true;
        }
    }

    private void OnTriggerExit (Collider other)
    {
        Health otherHealth = other.gameObject.GetComponent<Health>();
        if (otherHealth != null && otherHealth.Team == 0) // player
        {
            Debug.Log("UN - triggering");
            inRange = false;
        }
    }
}
