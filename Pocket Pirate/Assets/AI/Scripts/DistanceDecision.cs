using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decision/Distance")]
public class DistanceDecision : Decision
{
    public Vector3Reference target;
    public float DistanceEnterThreshold = 5f;
    public float DistanceExitThreshold = 10f;


    public override bool Decide (StateController controller)
    {
        return Distance(controller);
    }

    private bool Distance(StateController controller)
    {
        if (!controller.isWithin && Vector3.Distance(controller.transform.position, target.Value) < DistanceEnterThreshold)
            controller.isWithin = true;
        else if (controller.isWithin && Vector3.Distance(controller.transform.position, target.Value) > DistanceExitThreshold)
            controller.isWithin = false;

        return controller.isWithin;
    }
}
