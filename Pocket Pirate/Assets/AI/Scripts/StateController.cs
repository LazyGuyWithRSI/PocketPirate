using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public State currentState;
    public State remainState;

    public Vector3Reference playerPos;

    [HideInInspector] public IBoatMover mover;

    [HideInInspector] public bool canAct; // TODO coupling
    [HideInInspector] public bool isWithin; // TODO coupling
    [HideInInspector] public float stateTimeElapsed;

    // temporary, move to a shoot controller (that can be asked for shooting angles, ranges, reloads, etc, so states can use it to generically shoot anything)
    public GameObject starboardShootMount;
    [HideInInspector] public IShooter starboardShooter;
    public GameObject portShootMount;
    [HideInInspector] public IShooter portShooter;

    [HideInInspector] public Dictionary<Type, object> contexts;

    private bool aiActive;


    void Awake()
    {
        starboardShooter = starboardShootMount.GetComponentInChildren<IShooter>();
        portShooter = portShootMount.GetComponentInChildren<IShooter>();
        mover = GetComponent<IBoatMover>();

        contexts = new Dictionary<Type, object>();

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

    void OnDrawGizmos ()
    {
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
            Debug.Log("Changing to " + nextState.name);
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
    }
}
