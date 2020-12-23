using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public State currentState;
    public State remainState;

    public Vector3Reference playerPos;

    [HideInInspector] public IBoatMover mover;

    [HideInInspector] public bool canAct;
    [HideInInspector] public float stateTimeElapsed;

    // temporary, move to a shoot controller (that can be asked for shooting angles, ranges, reloads, etc, so states can use it to generically shoot anything)
    public GameObject starboardShootMount;
    [HideInInspector] public IShooter starboardShooter;
    public GameObject portShootMount;
    [HideInInspector] public IShooter portShooter;

    private bool aiActive;


    void Awake()
    {
        starboardShooter = starboardShootMount.GetComponentInChildren<IShooter>();
        portShooter = portShootMount.GetComponentInChildren<IShooter>();
        mover = GetComponent<IBoatMover>();

        canAct = true;
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
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
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

    public void StartActCooldown(float duration)
    {
        StartCoroutine(CanActCooldown(duration));
    }

    private IEnumerator CanActCooldown (float duration)
    {
        canAct = false;
        yield return new WaitForSeconds(duration);
        canAct = true;
    }

}
