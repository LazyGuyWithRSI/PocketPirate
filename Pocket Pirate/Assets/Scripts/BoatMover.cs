using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMover : MonoBehaviour, IBoatMover
{
    public float Speed = 10f;
    public float TurnSpeed = 2f;
    public float TurnSpeedWhileDrifting = 3f;
    public float DriftDuration = 0.5f;
    public float DriftForwardSpeed = 10f;
    public float EasingFactor = 1f;
    public float WaterDrag = 0.8f;

    private int moveDirection = 1;
    private float desiredHeading = 0f;
    private bool setHeadingToCurrent = false;
    private float turnDirection = 0f;
    private bool usingManualTurning = false;
    private bool isDead = false;
    private bool inDrift = false;

    private float currentTurnSpeed = 0f;

    private Vector3 moveVector;
    private int requestedDirection = 1;

    private Rigidbody rb;
    private IJump jump;

    public void SetMoving (int direction)
    {
        // if airborne, don't change move
        //if (!jump.IsAirborne())
        //moveDirection = Mathf.Clamp(direction, -1, 1);
        requestedDirection = direction;
    }

    public void SetHeading (float direction)
    {
        desiredHeading = (direction % 360);
        if (desiredHeading < 0)
            desiredHeading = 360 - desiredHeading;

        usingManualTurning = false;
    }

    public void SetTurnDirection(float direction)
    {
        turnDirection = Mathf.Clamp(direction, -1, 1);
        usingManualTurning = true;
    }

    public void SetHeadingToCurrent()
    {
        setHeadingToCurrent = true;
        usingManualTurning = false;
    }

    public float GetHeading()
    {
        float currentHeading = (transform.eulerAngles.y % 360);
        if (currentHeading < 0)
            currentHeading = 360 - currentHeading;
        return currentHeading;
    }

    public int GetMoving() { return moveDirection; }

    public void Dead()
    {
        isDead = true;
    }

    public void Drift(bool isStart)
    {
        if (isStart)
            StartCoroutine(DriftCoroutine(DriftDuration));
        else
        {
            StopAllCoroutines();
            currentTurnSpeed = TurnSpeed;
            SetMoving(1);
            inDrift = false;
        }
    }

    private IEnumerator DriftCoroutine (float duration)
    {
        inDrift = true;
        currentTurnSpeed = TurnSpeedWhileDrifting;
        yield return new WaitForSeconds(duration / 2);
        SetMoving(0);
        yield return new WaitForSeconds(duration / 2);
        currentTurnSpeed = TurnSpeed;
        inDrift = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = GetComponent<IJump>();

        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;

        currentTurnSpeed = TurnSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // check requested direction
        if (!jump.IsAirborne() && !inDrift) // don't change move vector if we are in the air
        { 
            moveVector = transform.forward;

            if (requestedDirection != moveDirection)
                moveDirection = requestedDirection;
        }

        // move boat forward

        if (inDrift && !jump.IsAirborne())
            rb.AddForce(Vector3.Lerp(moveVector, transform.forward, 0.5f) * Speed * moveDirection);
        else
            rb.AddForce(moveVector * Speed * moveDirection);

        // turn boat towards target heading
        // TODO maybe move heading calculation to helper class (if needed elsewhere)

        if (!usingManualTurning)
        {
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

            turnDirection = 0;
            if (headingDifference > 0)
                turnDirection = 1;
            else
                turnDirection = -1;


            turnDirection *= Mathf.Clamp(Mathf.Abs(headingDifference / EasingFactor), -1, 1);
        }
        //Debug.Log("current heading: " + currentHeading + ", target: " + desiredHeading + ", dif: " + headingDifference);
        float forwardVelocity = Mathf.Max(transform.InverseTransformDirection(rb.velocity).z, (float)(Mathf.Abs(transform.InverseTransformDirection(rb.velocity).x) * 0.7));

        rb.AddRelativeTorque(Vector3.up * currentTurnSpeed * turnDirection * forwardVelocity);

        Vector3 vel = rb.velocity;
        vel.x *= WaterDrag;
        vel.z *= WaterDrag;
        rb.velocity = vel;


        if (!isDead)
            rb.transform.localEulerAngles = new Vector3(0, rb.transform.localEulerAngles.y, 0);
    }
}

public interface IBoatMover
{
    void SetHeading (float direction);
    void SetMoving (int direction);
    int GetMoving ();
    void SetTurnDirection (float direction);
    float GetHeading ();
    void SetHeadingToCurrent ();
    void Drift (bool isStart);
    void Dead ();
}