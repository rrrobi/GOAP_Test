using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_GetFuel : GOAPGoal
{
    int targetFuelCount = 10;

    public Goal_GetFuel(string goalName, Agent agent)
        : base(goalName, agent)
    {
        addGoalState("FuelStocked", true);
    }

    public override bool CanRun()
    {
        return base.CanRun();
    }
    public override int CalculatePriority()
    {
        return (Mathf.Max((targetFuelCount - Agent.CurrentFuel), 0) * 10) + activeGoalPriorityModifier;
    }
    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
    }
    public override void OnGoalDeactivated()
    {
        base.OnGoalDeactivated();
    }
}
