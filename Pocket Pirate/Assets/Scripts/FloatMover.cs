using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatMover : MonoBehaviour
{
    public float FloatHeight = 5f;

    private Rigidbody rb;
    private Health health;

    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAlive)
        {
            transform.position = new Vector3(transform.position.x, FloatHeight, transform.position.z);

            if (health.curHealth <= 0)
            {
                isAlive = false;
                rb.useGravity = true;
            }
        }
    }
}
