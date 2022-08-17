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
    private HashSet<KeyValuePair<string, object>> preconditions;
    private HashSet<KeyValuePair<string, object>> effects;

    public string GoalName { get; protected set; }
    public Agent Agent { get; protected set; }

    // OnTick() - Do we need to adjust a Goal's Data over time?
    // Activate()
    // Deactivate()

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

    }
    public virtual void OnGoalDeactivated()
    {

    }

}
