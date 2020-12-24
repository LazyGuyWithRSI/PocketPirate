using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decision/Distance")]
public class DistanceDecision : Decision
{
    public Vector3Reference target;
    public float DistanceEnterThreshold = 5f;
    public float DistanceExitThreshold = 10f;

    private DistanceDecisionContext context;

    public override bool Decide (StateController controller)
    {
        if (!controller.contexts.ContainsKey(typeof(DistanceDecisionContext)))
            controller.contexts.Add(typeof(DistanceDecisionContext), new DistanceDecisionContext() { isWithin = false });

        object obj;
        controller.contexts.TryGetValue(typeof(DistanceDecisionContext), out obj);
        context = (DistanceDecisionContext)obj;
        return Distance(controller);
    }

    private bool Distance(StateController controller)
    {
        if (!context.isWithin && Vector3.Distance(controller.transform.position, target.Value) < DistanceEnterThreshold)
            context.isWithin = true;
        else if (context.isWithin && Vector3.Distance(controller.transform.position, target.Value) > DistanceExitThreshold)
            context.isWithin = false;

        return context.isWithin;
    }
}

public class DistanceDecisionContext
{
    public bool isWithin = false;
}