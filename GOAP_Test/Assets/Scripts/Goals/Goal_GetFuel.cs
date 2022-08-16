using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_GetFuel : GOAPGoal
{
    int targetFuelCount = 10;

    public Goal_GetFuel(string goalName, Agent agent)
        : base(goalName, agent)
    {

    }

    public override bool CanRun()
    {
        return base.CanRun();
    }
    public override int CalculatePriority()
    {
        return Mathf.Max((targetFuelCount - Agent.CurrentFood), 0) * 10;
    }
}
