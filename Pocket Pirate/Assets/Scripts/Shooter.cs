using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IShooter
{
    public GameObject ProjectilePrefab;
    public FloatReference ShootForce;
    public FloatReference UpShootForce;

    private ParticleSystem smoke;

    public bool Shoot ()
    {
        smoke.Play();
        GameObject obj = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * ShootForce.Value);
        rb.AddForce(Vector3.up * UpShootForce.Value);

        return true;
    }

    public bool CanShoot()
    {
        return true;
    }

    void Start()
    {
        smoke = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        
    }

    public void SetName (string Name)
    {
        throw new System.NotImplementedException();
    }

    public float GetShootHeading()
    {
        return 0;
    }
}

public interface IShooter
{
    float GetShootHeading ();
    bool Shoot ();
    bool CanShoot ();
    void SetName (string Name);
}