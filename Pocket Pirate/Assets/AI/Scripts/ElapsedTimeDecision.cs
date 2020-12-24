using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decision/ElapsedTime")]
public class ElapsedTimeDecision : Decision
{
    public float time = 5f;

    public override bool Decide (StateController controller)
    {
        Debug.Log("controller.stateTimeElapsed " + controller.stateTimeElapsed);
        if (controller.stateTimeElapsed < time)
            return false;

        return true;
    }
}
