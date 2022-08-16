using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_Rest : GOAPGoal
{
    int priority = 10;
    public Goal_Rest(string goalName, Agent agent)
        : base(goalName, agent)
    {

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
