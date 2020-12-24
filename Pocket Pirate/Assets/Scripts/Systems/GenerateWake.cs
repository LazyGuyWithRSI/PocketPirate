using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWake : MonoBehaviour
{
    public Transform target;
    public float HeightOffWater = 0.2f;
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
        if (target == null)
            Destroy(gameObject);

        if (target.transform.position.y > 0 && particleSys.isEmitting)
        {
            particleSys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        else if (target.transform.position.y <= 0 && !particleSys.isEmitting)
        {
            Debug.Log("playing wake");
            particleSys.Play();
        }

        transform.position = new Vector3(target.position.x + XOffset, HeightOffWater, target.position.z + ZOffset);
    }
}
