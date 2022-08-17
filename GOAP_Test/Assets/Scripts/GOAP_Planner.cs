using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GOAP_Planner 
{
   

    public static GOAPAction Plan(Agent agent,
                           HashSet<GOAPAction> availableActions,
                           HashSet<KeyValuePair<string, object>> goalState)
    {
        // Narrow list of actions to only ones that are possible
        HashSet<GOAPAction> possibleActions = new HashSet<GOAPAction>();
        foreach (var action in availableActions)
        {
            // Reset each action, to ensure we do not have setting from previous use
            action._Reset();
            // Is action possible?
            if (action.CheckSpecificPrecondition(agent))
                possibleActions.Add(action);
        }

        // Will select 1 action that meets criteria, Doesn't take cost into account
        // Will only select a single action that meets ALL criteria, will not combine multple actions
        GOAPAction actionChosen = null;
        foreach (var action in possibleActions)            
        {
            bool FullMatch = false;
            foreach (var goal in goalState)
            {

                if (!action.Effects.Contains(goal)) // If we find ANY goals not in the Action's effects, it doesn't meet ALL requirements, move on to the next action
                {
                    FullMatch = false;
                    break;
                }
                else // so far so good, fullMatch = true untill we find a goal the action does not achieve
                    FullMatch = true;
            }
            if (FullMatch)
            {
                actionChosen = action;
                break;
            }
        }
        // Return selected Action
        if (actionChosen != null)
            return actionChosen;

        // failed to create plan - return null
        return null;
    }
}
