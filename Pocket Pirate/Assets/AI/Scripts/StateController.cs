using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public State currentState;
    public State remainState;

    public Vector3Reference playerPos;
    public FloatReference playerHeading;

    [HideInInspector] public IBoatMover mover;

    [HideInInspector] public bool canAct; // TODO coupling
    [HideInInspector] public bool isWithin; // TODO coupling
    [HideInInspector] public float stateTimeElapsed;

    public GameObject[] ShooterGameObjects;
    [HideInInspector] public List<IShooter> shooters;

    [HideInInspector] public Dictionary<Type, object> contexts;

    [HideInInspector] public Vector3 ShootTarget; // TODO remove this debug shit

    private bool aiActive;


    void Awake()
    {
        mover = GetComponent<IBoatMover>();

        contexts = new Dictionary<Type, object>();

        shooters = new List<IShooter>();
        foreach (GameObject go in ShooterGameObjects)
            shooters.Add(go.GetComponent<IShooter>());

        canAct = true;
        isWithin = true;
    }

    void Start ()
    {
        SetupAI();
    }

    public void SetupAI()//bool aiActivationFromManager
    {
        aiActive = true;
    }

    public void StopAI()
    {
        aiActive = false;
    }

    void OnDrawGizmos ()
    {
        if (ShootTarget != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ShootTarget, 1);
        }

        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }

    public void Update ()
    {
        if (!aiActive)
            return;

        currentState.UpdateState(this);
        stateTimeElapsed += Time.deltaTime;
    }

    // return true if new state is the same as the old
    public bool TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            remainState = nextState;
            currentState = nextState;
            OnExitState();
            return false;
        }

        return true;
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    public void OnExitState()
    {
        stateTimeElapsed = 0;

        ShootTarget = Vector3.zero;
    }
}
