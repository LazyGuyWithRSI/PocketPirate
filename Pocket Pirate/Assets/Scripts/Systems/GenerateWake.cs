using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWake : MonoBehaviour
{
    public Transform target;

    public float XOffset = 0.0f;
    public float ZOffset = 0.0f;

    private ParticleSystem particleSys;

    // Start is called before the first frame update
    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("particle playing: " + particleSys.isEmitting);
        if (target.transform.position.y > 0 && particleSys.isEmitting)
        {
            particleSys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        else if (target.transform.position.y <= 0 && !particleSys.isEmitting)
        {
            Debug.Log("playing wake");
            particleSys.Play();
        }

        transform.position = new Vector3(target.position.x + XOffset, 0.3f, target.position.z + ZOffset);
    }
}
