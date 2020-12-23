using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/Patrol")]
public class PatrolAction : Action
{
    public float DelayLower = 3f;
    public float DelayUpper = 8f;

    public override void Act (StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        if (!controller.canAct)
            return;

        // move in random direction
        float newHeading = Random.Range(0, 360);
        controller.mover.SetHeading(newHeading);
        Debug.Log("Patrol, new heading " + newHeading);

        controller.StartActCooldown(Random.Range(DelayLower, DelayUpper));
    }
}
