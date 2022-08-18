using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_Idle : GOAPGoal
{
    int priority = 35;
    public Goal_Idle(string goalName, Agent agent)
        : base(goalName, agent)
    {
        addGoalState("IsIdle", true);
    }

    public override bool CanRun()
    {
        return true;
    }
    public override int CalculatePriority()
    {
        return priority;
    }
}
