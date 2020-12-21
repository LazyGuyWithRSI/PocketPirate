using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatInput : MonoBehaviour
{
    public float JumpAccelRequired = 0.5f;

    public Vector2Reference JoyStickInput;
    public IBoatMover mover;
    public IJump jumper;

    private bool stickReleasedThisFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<IBoatMover>();
        jumper = GetComponent<IJump>();

        // TODO sub to jump event or something
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) || Input.acceleration.z > JumpAccelRequired) // DEBUG
        {
            Debug.Log("Request Jump");
            jumper.DoJump();
        }

        /* DEBUG
        if (Input.GetKey(KeyCode.A))
            mover.SetHeading(90);
        else if (Input.GetKey(KeyCode.W))
            mover.SetHeading(180);
        else if (Input.GetKey(KeyCode.D))
            mover.SetHeading(270);
        else if (Input.GetKey(KeyCode.S))
            mover.SetHeading(0);
        */

        if (JoyStickInput.Value.x == 0 && JoyStickInput.Value.y == 0)
        {
            mover.SetHeadingToCurrent();
            return;
        }

        float angle = Mathf.Atan2(JoyStickInput.Value.x, JoyStickInput.Value.y) * 180 / Mathf.PI;
        angle += 180;

        mover.SetHeading(angle);

        

    }
}
