using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class GOAP_Planner 
{
   

    public static Queue<GOAPAction> Plan(Agent agent,
                           HashSet<GOAPAction> availableActions,
                           HashSet<KeyValuePair<string, object>> goalState,
                           HashSet<KeyValuePair<string, object>> CurrentState)
    {
        // Narrow list of actions to only ones that are possible
        HashSet<GOAPAction> possibleActions = new HashSet<GOAPAction>();
        foreach (var action in availableActions)
        {
            // Reset each action, to ensure we do not have setting from previous use
            action.DoReset();
            // Is action possible?
            if (action.CheckSpecificPrecondition(agent))
                possibleActions.Add(action);
        }

        // List of nodes (nodes represent Actions)
        List<Node> leaves = new List<Node>();

        // Start node is the sate we are in now
        Node start = new Node(null, 0, CurrentState, null, agent.transform.position);
        // populate List of leaf nodes, Each leaf will give a path from Current state to Goal state
        // Each leaf will have a total cost of that path
        bool success = BuildBranch(start, leaves, possibleActions, goalState);
        if (!success)
        {
            // No possible plan found among available actions
            Debug.Log("No Plan found to achieve goal");
            return null;
        }

        // get the cheapest leaf
        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else
            {
                if (leaf.runningCost < cheapest.runningCost)
                    cheapest = leaf;
            }
        }

        // get its node and work back through the parents
        // We must add to list first so we can build the queue correctly
        List<GOAPAction> actionList = new List<GOAPAction>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                actionList.Insert(0, n.action); // insert the action at the front of the list
            }
            n = n.parent;
        }
        // we now have this action list in correct order
        Queue<GOAPAction> queue = new Queue<GOAPAction>();
        foreach (GOAPAction a in actionList)
        {
            queue.Enqueue(a);
        }

        // return queue for use by Agent.
        return queue;
    }

    /// <summary>
    /// Returns true if any solutions are found
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="leaves"></param>
    /// <param name="usableActions"></param>
    /// <param name="goalState"></param>
    /// <returns></returns>
    private static bool BuildBranch(Node parent, List<Node> leaves, HashSet<GOAPAction> usableActions, HashSet<KeyValuePair<string, object>> goalState)
    {
        bool foundOne = false;

        // go through each action available at this node and see if we can use it here
        foreach (GOAPAction action in usableActions)
        {
            // if the parent state has the conditions for this action's preconditions, we can use it here
            if (IsFullStateMatch(action.Preconditions, parent.state))
            {
                float dist = Vector3.Distance(parent.position, action.transform.position);

                // apply the action's effects to the parent state
                HashSet<KeyValuePair<string, object>> currentState = AmendState(parent.state, action.Effects);                
                Node node = new Node(parent, parent.runningCost + action.GetCostWithDistance(dist), currentState, action, action.transform.position);

                if (GoalInState(goalState, currentState))
                {
                    // we found a solution!
                    leaves.Add(node);
                    foundOne = true;
                }
                else
                {
                    // test all the remaining actions and branch out the tree
                    HashSet<GOAPAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildBranch(node, leaves, subset, goalState);
                    if (found)
                        foundOne = true;
                }
            }
        }

        return foundOne;
    }

    /// <summary>
    /// Returns New set of actions with the supplied removed.
    /// </summary>
    /// <param name="actions"></param>
    /// <param name="removeMe"></param>
    /// <returns></returns>
    private static HashSet<GOAPAction> ActionSubset(HashSet<GOAPAction> actions, GOAPAction removeMe)
    {
        HashSet<GOAPAction> subset = new HashSet<GOAPAction>();
        foreach (GOAPAction a in actions)
        {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }
    /// <summary>
    /// Returns True if ANY of the Goal states are matched with supplied 'newState'
    /// </summary>
    /// <param name="goalState"></param>
    /// <param name="newState"></param>
    /// <returns></returns>
    private static bool GoalInState(HashSet<KeyValuePair<string, object>> goalState, HashSet<KeyValuePair<string, object>> newState)
    {
        // ToDo... This will not work for goals with multiple requirements,
        bool match = false;
        foreach (KeyValuePair<string, object> t in goalState)
        {
            foreach (KeyValuePair<string, object> s in newState)
            {
                if (s.Equals(t))
                {
                    match = true;
                    break;
                }
            }
        }
        return match;
    }
    /// <summary>
    /// Returns true is ALL states in 'goalState' are found and matched within 'newState'
    /// </summary>
    /// <param name="goalState"></param>
    /// <param name="newState"></param>
    /// <returns></returns>
    private static bool IsFullStateMatch(HashSet<KeyValuePair<string, object>> goalState, HashSet<KeyValuePair<string, object>> newState)
    {
        bool allMatch = true;
        foreach (KeyValuePair<string, object> t in goalState)
        {
            bool match = false;
            foreach (KeyValuePair<string, object> s in newState)
            {
                if (s.Equals(t))
                {
                    match = true;
                    break;
                }
            }
            if (!match)
                allMatch = false;
        }
        return allMatch;
    }
    /// <summary>
    /// Returns new set of States equal to 'currentState', amended to match any States found in 'stateChange'
    /// </summary>
    /// <param name="currentState"></param>
    /// <param name="stateChange"></param>
    /// <returns></returns>
    private static HashSet<KeyValuePair<string, object>> AmendState(HashSet<KeyValuePair<string, object>> currentState, HashSet<KeyValuePair<string, object>> stateChange)
    {
        HashSet<KeyValuePair<string, object>> state = new HashSet<KeyValuePair<string, object>>();
        // copy the KVPs over as new objects
        foreach (KeyValuePair<string, object> s in currentState)
        {
            state.Add(new KeyValuePair<string, object>(s.Key, s.Value));
        }

        foreach (KeyValuePair<string, object> change in stateChange)
        {
            // if the key exists in the current state, update the Value
            bool exists = false;

            foreach (KeyValuePair<string, object> s in state)
            {
                if (s.Key.Equals(change.Key))
                {
                    exists = true;
                    break;
                }
            }

            if (exists)
            {
                state.RemoveWhere((KeyValuePair<string, object> kvp) => { return kvp.Key.Equals(change.Key); });
                KeyValuePair<string, object> updated = new KeyValuePair<string, object>(change.Key, change.Value);
                state.Add(updated);
            }
            // if it does not exist in the current state, add it
            else
            {
                state.Add(new KeyValuePair<string, object>(change.Key, change.Value));
            }
        }
        return state;
    }



    public static GOAPAction SimplePlan(Agent agent,
                           HashSet<GOAPAction> availableActions,
                           HashSet<KeyValuePair<string, object>> goalState)
    {
        // Narrow list of actions to only ones that are possible
        HashSet<GOAPAction> possibleActions = new HashSet<GOAPAction>();
        foreach (var action in availableActions)
        {
            // Reset each action, to ensure we do not have setting from previous use
            action.DoReset();
            // Is action possible?
            if (action.CheckSpecificPrecondition(agent))
                possibleActions.Add(action);
        }


        // Will only select a single action that meets ALL criteria, will not combine multple actions
        // Has no way to chain actions that have their own preconditions
        List<GOAPAction> usefulActions = new List<GOAPAction>();
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
                //actionChosen = action;
                usefulActions.Add(action);
                //break;
            }
        }

        // Use costs to pick whcih action is best.
        float cost = float.MaxValue;
        GOAPAction actionChosen = null;
        foreach (var action in usefulActions)
        {
            float dist = Vector3.Distance(agent.transform.position, action.transform.position);
            float newCost = action.GetCostWithDistance(dist);
            if (newCost < cost)
            {
                cost = newCost;
                actionChosen = action;
            }
        }


        // Return selected Action
        if (actionChosen != null)
            return actionChosen;

        // failed to create plan - return null
        return null;
    }



    protected class Node
    {
        public Node parent;
        public float runningCost;
        public HashSet<KeyValuePair<string, object>> state;
        public GOAPAction action;
        public Vector3 position;

        public Node(Node parent, float runningCost, HashSet<KeyValuePair<string, object>> state, GOAPAction action, Vector3 position)
        {
            this.parent = parent;
            this.runningCost = runningCost;
            this.state = state;
            this.action = action;
            this.position = position;
        }
    }
}
