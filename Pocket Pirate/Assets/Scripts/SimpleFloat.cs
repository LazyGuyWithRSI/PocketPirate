using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFloat : MonoBehaviour
{
    public float buoyancy = 1f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // TODO use actual water height. For now it is always 0
        float force = Mathf.Max(0,-transform.position.y - 0.4f) * buoyancy;
        rb.AddForce(Vector3.up *( force * Time.fixedDeltaTime));
    }
}
