using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosUpdater : MonoBehaviour
{
    public Vector3Reference playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition.Value = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition.Value = transform.position;
    }
}
