using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Shoot")]
public class ShootAction : Action
{
    public float AngleRequired = 10f;

    public override void Act (StateController controller)
    {
        
        float desiredAngle = AimAtPlayer(controller);
        TryShoot(controller, desiredAngle);
    }

    private void TryShoot(StateController controller, float desiredAngle)
    {
        if (Mathf.Abs(desiredAngle - controller.mover.GetHeading()) < AngleRequired && controller.starboardShooter.CanShoot())
        {
            controller.starboardShooter.Shoot();
        }
    }

    private float AimAtPlayer(StateController controller)
    {
        Vector3 playerPos = controller.playerPos.Value - controller.transform.position;
        float angle = Mathf.Atan2(playerPos.x, playerPos.z) * (180 / Mathf.PI);
        angle -= 90;
        if (angle < 0)
            angle += 360;

        controller.mover.SetHeading(angle);
        return angle;
    }
}
