using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Shoot")]
public class ShootAction : Action
{
    public float AngleRequired = 10f;
    public float PredictionDistance = 8f;
    public float AimDeviation = 3f;

    private ShootActionContext context;

    public override void Act (StateController controller)
    {
        if (!controller.contexts.ContainsKey(typeof(ShootActionContext)))
            controller.contexts.Add(typeof(ShootActionContext), new ShootActionContext());

        object obj;
        controller.contexts.TryGetValue(typeof(ShootActionContext), out obj);
        context = (ShootActionContext)obj;

        float desiredAngle = AimAtPlayer(controller);
        TryShoot(controller, desiredAngle);
    }

    private void TryShoot(StateController controller, float desiredAngle)
    {
        foreach (IShooter shooter in controller.shooters)
        {
            float shooterAngle = desiredAngle + shooter.GetShootHeading();
            if (shooterAngle < 0)
                shooterAngle += 360;
            else if (shooterAngle > 360)
                shooterAngle -= 360;


            //Debug.Log("Looking to shoot at heading " + shooterAngle + ", my heading " + controller.mover.GetHeading());
            if (Mathf.Abs(shooterAngle - controller.mover.GetHeading()) < AngleRequired && shooter.CanShoot())
            {
                if (shooter.Shoot())
                    context.Variance = Vector3.zero;
            }

            // turrets
            if (shooter is Turret)
            {
                Vector3 prediction = (Utils.Vector3FromHeading(controller.playerHeading.Value) * PredictionDistance);

                prediction += context.Variance;
                Vector3 playerPos = (controller.playerPos.Value + prediction) - (shooter as Turret).transform.position;

                float angle = Mathf.Atan2(playerPos.x, playerPos.z) * (180 / Mathf.PI);
                //angle -= 90;
                if (angle < 0)
                    angle += 360;

                //controller.mover.SetHeading(angle);
                float turretDesiredAngle = angle;
                shooterAngle = turretDesiredAngle + shooter.GetShootHeading();
                if (shooterAngle < 0)
                    shooterAngle += 360;
                else if (shooterAngle > 360)
                    shooterAngle -= 360;

                float aimAngle = turretDesiredAngle + controller.mover.GetHeading();
                if (aimAngle > 360)
                    aimAngle -= 360;

                shooter.SetAimHeading(shooterAngle - controller.mover.GetHeading());
                Debug.Log("Turret heading: " + shooter.GetShootHeading());
                if (Mathf.Abs(shooterAngle - controller.mover.GetHeading()) < AngleRequired && shooter.CanShoot())
                {
                    if (shooter.Shoot())
                        context.Variance = Vector3.zero;
                }
            }
        }

        /*
        if (Mathf.Abs(desiredAngle - controller.mover.GetHeading()) < AngleRequired && controller.starboardShooter.CanShoot())
        {
            if (controller.starboardShooter.Shoot())
                context.Variance = Vector3.zero;
        }*/
    }

    private float AimAtPlayer(StateController controller)
    {
        Vector3 prediction = (Utils.Vector3FromHeading(controller.playerHeading.Value) * PredictionDistance);

        if (context.Variance == Vector3.zero)
        {
            context.Variance = Utils.RandomPointInCircle(AimDeviation);
            context.Variance.y = 0;
        }

        prediction += context.Variance;

        controller.ShootTarget = controller.playerPos.Value + prediction;
        Vector3 playerPos = (controller.playerPos.Value + prediction) - controller.transform.position;

        float angle = Mathf.Atan2(playerPos.x, playerPos.z) * (180 / Mathf.PI);
        //angle -= 90;
        if (angle < 0)
            angle += 360;

        //controller.mover.SetHeading(angle);
        return angle;
    }
}

public class ShootActionContext
{
    public Vector3 Variance = Vector3.zero;
}