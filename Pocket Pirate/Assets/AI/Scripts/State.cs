using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State")]
public class State : ScriptableObject
{
    public Action[] actions;
    public Transition[] transitions;
    public Color sceneGizmoColor = Color.grey;

    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    public void DoActions(StateController controller)
    {
        foreach (Action action in actions)
            action.Act(controller);
    }

    public void CheckTransitions(StateController controller)
    {
        foreach (Transition transition in transitions)
        {
            bool decisionSucceeded = transition.decision.Decide(controller);

            if (decisionSucceeded)
            {
                if (!controller.TransitionToState(transition.trueState))
                    break;
            }
            else
            {
                if (!controller.TransitionToState(transition.falseState))
                    break;
            }
        }
    }


}
