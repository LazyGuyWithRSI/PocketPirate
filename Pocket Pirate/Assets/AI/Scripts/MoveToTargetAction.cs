using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/MoveToTarget")]
public class MoveToTargetAction : Action
{
    // target is just the player

    public override void Act (StateController controller)
    {
        MoveToPlayer(controller);
    }

    private void MoveToPlayer (StateController controller)
    {
        Vector3 playerPos = controller.playerPos.Value - controller.transform.position;
        float angle = Mathf.Atan2(playerPos.x, playerPos.z) * (180 / Mathf.PI);
        if (angle < 0)
            angle += 360;

        Debug.Log("angle: " + angle);
        controller.mover.SetHeading(angle);
    }
}