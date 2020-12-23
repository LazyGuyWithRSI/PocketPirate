using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMover : MonoBehaviour, IBoatMover
{
    public float Speed = 10f;
    public float TurnSpeed = 2f;
    public float EasingFactor = 1f;

    private int moveDirection = 1;
    private float desiredHeading = 0f;
    private bool setHeadingToCurrent = false;

    private Rigidbody rb;

    public void SetMoving (int direction)
    {
    }

    public void SetHeading (float direction)
    {
        desiredHeading = (direction % 360);
        if (desiredHeading < 0)
            desiredHeading = 360 - desiredHeading;
    }

    public void SetHeadingToCurrent()
    {
        setHeadingToCurrent = true;
    }

    public float GetHeading()
    {
        float currentHeading = (transform.eulerAngles.y % 360);
        if (currentHeading < 0)
            currentHeading = 360 - currentHeading;
        return currentHeading;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // move boat forward
        rb.AddRelativeForce(Vector3.forward * Speed * moveDirection);

        // turn boat towards target heading
        // TODO maybe move heading calculation to helper class (if needed elsewhere)
        float currentHeading = (transform.eulerAngles.y % 360);
        if (currentHeading < 0)
            currentHeading = 360 - currentHeading;

        if (setHeadingToCurrent)
        {
            desiredHeading = currentHeading;
            setHeadingToCurrent = false;
        }

        float headingDifference = desiredHeading - currentHeading;

        if (headingDifference < 180)
            headingDifference = headingDifference + 360;

        if (headingDifference > 180)
            headingDifference = headingDifference - 360;

        float turnDirection = 0;
        if (headingDifference > 0)
            turnDirection = 1;
        else
            turnDirection = -1;

        turnDirection *= Mathf.Clamp(Mathf.Abs(headingDifference / EasingFactor), -1, 1);
        //Debug.Log("current heading: " + currentHeading + ", target: " + desiredHeading + ", dif: " + headingDifference);

        rb.AddRelativeTorque(Vector3.up * TurnSpeed * turnDirection);
    }
}

public interface IBoatMover
{
    void SetHeading (float direction);
    void SetMoving (int direction);
    float GetHeading ();
    void SetHeadingToCurrent ();
}