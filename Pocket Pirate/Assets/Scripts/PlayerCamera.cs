using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Vector3Reference PlayerPosition;

    public float FollowLerp = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // follow
        transform.position = Vector3.Lerp(transform.position, PlayerPosition.Value, FollowLerp);
        
        /*
        // look
        var targetRotation = Quaternion.LookRotation(transform.position - playerTransform.Value.position);
        Vector3 eulers = targetRotation.eulerAngles;
        eulers.z = 0;
        eulers.x = 0;
        transform.eulerAngles = eulers;
        */
    }
}
