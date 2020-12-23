using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/Patrol")]
public class PatrolAction : Action
{
    public float DelayLower = 3f;
    public float DelayUpper = 8f;

    private PatrolActionContext context;

    public override void Act (StateController controller)
    {

        if (!controller.contexts.ContainsKey(typeof(PatrolActionContext)))
            controller.contexts.Add(typeof(PatrolActionContext), new PatrolActionContext() { canAct = true });

        object obj;
        controller.contexts.TryGetValue(typeof(PatrolActionContext), out obj);
        context = (PatrolActionContext)obj;

        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        if (!context.canAct)
            return;

        // move in random direction
        float newHeading = Random.Range(0, 360);
        controller.mover.SetHeading(newHeading);
        //Debug.Log("Patrol, new heading " + newHeading);

        controller.StartCoroutine(CanActCooldown(context, Random.Range(DelayLower, DelayUpper)));
    }

    private IEnumerator CanActCooldown (PatrolActionContext context, float duration)
    {
        context.canAct = false;
        yield return new WaitForSeconds(duration);
        context.canAct = true;
    }
}

public class PatrolActionContext
{
    public bool canAct = true;
}
