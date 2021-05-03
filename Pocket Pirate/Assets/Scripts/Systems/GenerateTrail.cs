using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTrail : MonoBehaviour
{
    public TrailRenderer Trail;

    private bool isDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDisabled)
        {
            if (transform.position.y > 0.1 && Trail.emitting)
            {
                Trail.emitting = false;
            }
            else if (transform.position.y <= 0.1 && !Trail.emitting)
            {
                Trail.emitting = true;
            }
        }
    }

    public void StopEmitting()
    {
        isDisabled = true;
        Trail.emitting = false;
    }
}
