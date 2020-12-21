using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatAI : MonoBehaviour
{
    // TODO break these out?
    public GameObject starboardShootMount;
    private IShooter starboardShooter;
    public GameObject portShootMount;
    private IShooter portShooter;

    public IBoatMover mover;

    // Start is called before the first frame update
    void Start()
    {
        starboardShooter = starboardShootMount.GetComponentInChildren<IShooter>();
        portShooter = portShootMount.GetComponentInChildren<IShooter>();
        mover = GetComponent<IBoatMover>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 60) < 1)
        {
            mover.SetHeading(Random.Range(0, 360));
        }

        if (Random.Range(0, 120) < 1)
        {
            if (Random.Range(0, 2) == 0)
                starboardShooter.Shoot();
            else
                portShooter.Shoot();
        }
    }
}
