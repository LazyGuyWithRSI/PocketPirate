using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanOnTurn : MonoBehaviour
{
    public Transform model;
    public float MaxLeanAngle = 25f;

    public float AngularVelocityMax = 2.0f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float percentVelocity = Mathf.Clamp(rb.angularVelocity.y / AngularVelocityMax, -1f, 1f);
        float leanAngle = MaxLeanAngle * percentVelocity;
        model.transform.localEulerAngles = new Vector3(0, 0, leanAngle);
    }
}
