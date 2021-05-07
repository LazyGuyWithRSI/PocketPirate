using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IShooter
{
    public GameObject ProjectilePrefab;
    public FloatReference ShootForceReference;
    public FloatReference UpShootForce;

    [SerializeField] private UpgradablePropertyReference UpgradableShootForce;

    public float PitchLower = 0.9f;
    public float PitchUpper = 1.1f;

    private float shootForce;
    private ParticleSystem smoke;
    private AudioSource audio;

    public bool Shoot ()
    {
        smoke.Play();
        audio.pitch = Random.Range(PitchLower, PitchUpper);
        audio.Play();
        GameObject obj = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * shootForce);
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
        audio = GetComponentInChildren<AudioSource>();

        shootForce = ShootForceReference.Value;

        if (UpgradableShootForce != null)
            shootForce = UpgradableShootForce.Value;
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

    public void SetCooldown (float cooldown) { }
    public float GetCooldown () { return 0; }
}

public interface IShooter
{
    float GetShootHeading ();
    bool Shoot ();
    bool CanShoot ();
    void SetCooldown (float cooldown);
    float GetCooldown ();
    void SetName (string Name);
}