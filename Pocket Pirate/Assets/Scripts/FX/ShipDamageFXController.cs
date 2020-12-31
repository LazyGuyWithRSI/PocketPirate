using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDamageFXController : MonoBehaviour
{
    public FireDamageFXController[] damageFX; // TODO use interface for damage fx points

    public float[] breakpoints;

    private bool[] hitBreakpoints;

    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();

        hitBreakpoints = new bool[breakpoints.Length];
    }

    // Update is called once per frame
    void Update()
    {
        int next = NextBreakPoint();
        if (next < 0)
            return;

        if (health.curHealth <= breakpoints[next])
        {
            hitBreakpoints[next] = true;
            damageFX[next].StartFire();
        }
    }

    private int NextBreakPoint()
    {
        for (int i = 0; i < hitBreakpoints.Length; i++)
        {
            if (hitBreakpoints[i] == false)
                return i;
        }

        return -1;
    }
}
