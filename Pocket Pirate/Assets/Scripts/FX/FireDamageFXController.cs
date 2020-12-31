using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamageFXController : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private Light light;

    bool underWater = false;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        light = GetComponentInChildren<Light>();

        particleSystem.Stop();
        light.enabled = false;
    }

    public void StartFire()
    {
        particleSystem.Play();
        light.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!underWater && transform.position.y <= 0)
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            light.enabled = false;
            underWater = true;
            //GameObject.Destroy(gameObject, 4f);
        }
    }
}
