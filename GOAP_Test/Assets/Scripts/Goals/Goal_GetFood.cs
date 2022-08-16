using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_GetFood : GOAPGoal
{
    int targetFoodCount = 10;

    public Goal_GetFood(string goalName, Agent agent)
        : base(goalName, agent)
    {

    }

    public override bool CanRun()
    {
        return base.CanRun();
    }
    public override int CalculatePriority()
    {
        return Mathf.Max((targetFoodCount - Agent.CurrentFood), 0) * 10;
    }
    
}
