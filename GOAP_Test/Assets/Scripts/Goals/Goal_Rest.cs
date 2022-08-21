using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_Rest : GOAPGoal
{
    int priority = 30;
    int targetEnergyvalue = 20;
    public Goal_Rest(string goalName, Agent agent)
        : base(goalName, agent)
    {
        addGoalState("IsRested", true);
    }

    public override bool CanRun()
    {
        return base.CanRun();
    }
    public override int CalculatePriority()
    {
        return (Mathf.Max((targetEnergyvalue - Agent.CurrentEnergy), 0) * 5) + activeGoalPriorityModifier;
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
