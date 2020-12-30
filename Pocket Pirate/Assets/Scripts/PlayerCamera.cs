using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Vector3Reference PlayerPosition;
    public float FollowLerp = 1f;

    private bool gameIsOver = false;

    // Start is called before the first frame update
    void Start()
    {
        PubSub.RegisterListener<OnGameOver>(OnGameOver);
    }

    public void OnGameOver(object obj)
    {
        gameIsOver = true;
    }

    // Update is called once per frame
    void Update()
    {
        // follow
        if (gameIsOver)
            return;

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
