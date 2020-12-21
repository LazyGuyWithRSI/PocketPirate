using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadside : MonoBehaviour, IShooter
{
    private IShooter[] shooters;

    public void Shoot ()
    {
        PubSub.Publish<OnShootEvent>(new OnShootEvent() { Position = transform.position });

        //           v start from 1 because GetComponentsInChildren includes self for some reason
        for (int i = 1; i < shooters.Length; i++)
            shooters[i].Shoot();

    }

    // Start is called before the first frame update
    void Start()
    {
        shooters = GetComponentsInChildren<IShooter>();
        //Debug.Log("shooters:" + shooters.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
