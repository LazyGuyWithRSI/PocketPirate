using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosUpdater : MonoBehaviour
{
    public Vector3Reference PlayerPosition;
    public FloatReference PlayerHeading;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPosition.Value = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosition.Value = transform.position;
        PlayerHeading.Value = Utils.HeadingFromRotation(transform.eulerAngles.y);
    }
}
