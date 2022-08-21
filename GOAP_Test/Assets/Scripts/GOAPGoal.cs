using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoal
{
    bool CanRun();
    int CalculatePriority();
    void OnGoalActivated();
    void OnGoalDeactivated();
}

public class GOAPGoal : IGoal
{
    private HashSet<KeyValuePair<string, object>> goalState = new HashSet<KeyValuePair<string, object>>();

    public string GoalName { get; protected set; }
    public Agent Agent { get; protected set; }
    
    protected int activeGoalPriorityModifier = 0;
    // OnTick() - Do we need to adjust a Goal's Data over time?

    public GOAPGoal(string goalName, Agent agent)
    {
        GoalName = goalName;
        Agent = agent;
    }
     
    public virtual bool CanRun()
    {
        return false;
    }
    public virtual int CalculatePriority()
    {
        return -1;
    }
    public virtual void OnGoalActivated()
    {
        activeGoalPriorityModifier = 10;
    }
    public virtual void OnGoalDeactivated()
    {
        activeGoalPriorityModifier = 0;
    }

    public void addGoalState(string key, object value)
    {
        goalState.Add(new KeyValuePair<string, object>(key, value));
    }

    public void removeGoalState(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in goalState)
        {
            if (kvp.Key.Equals(key))
            {
                remove = kvp;
            }
            if (!default(KeyValuePair<string, object>).Equals(remove))
            {
                goalState.Remove(remove);
            }
        }
    }

    public HashSet<KeyValuePair<string, object>> GoalState
    {
        get
        {
            return goalState;
        }
    }
}
