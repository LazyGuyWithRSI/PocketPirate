using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    private Light light;

    public float RampUpTime = 100f;
    public float DecayTime = 500f;
    public float MaxBrightness = 20f;

    private bool active = false;
    private bool rampingUp = true;

    private float stepUp = 0f;
    private float stepDown = 0f;

    void Start()
    {
        light = GetComponentInChildren<Light>();
        light.intensity = 0;

        stepUp = MaxBrightness / RampUpTime;
        stepDown = MaxBrightness / DecayTime;

        // just trigger on start because I am lazy
        active = true;
    }

    void FixedUpdate()
    {
        if (active)
        {
            if (rampingUp)
            {
                light.intensity += stepUp * Time.fixedDeltaTime;
                if (light.intensity >= MaxBrightness)
                    rampingUp = false;
            }
            else
                light.intensity -= stepDown * Time.fixedDeltaTime;
        }
    }
}
