using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IShooter
{
    public GameObject ProjectilePrefab;
    public FloatReference ShootForce;
    public FloatReference UpShootForce;

    private ParticleSystem smoke;

    public void Shoot ()
    {
        smoke.Play();
        GameObject obj = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * ShootForce.Value);
        rb.AddForce(Vector3.up * UpShootForce.Value);

    }

    void Start()
    {
        smoke = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        
    }
}

public interface IShooter
{
    void Shoot ();
}