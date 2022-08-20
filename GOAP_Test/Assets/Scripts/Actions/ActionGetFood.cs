using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGetFood : GOAPAction
{
    bool isFoodRestocked = false;

    // Start is called before the first frame update
    void Start()
    {
        addEffect("FoodStocked", true);
        AddPrecondition("HasFoodBasket", true);
        BaseCost = Cost = 15f;        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void _Reset()
    {
        isFoodRestocked = false;
    }
    protected override void ResetActionTime()
    {
        actionTime = 1;
    }
    public override bool IsDone() { return isFoodRestocked; }
    public override bool RequiresInRange() { return true; }
    public override bool CheckSpecificPrecondition(Agent agent)
    {
        // Can we collect more food?
        if (agent.CurrentFood < 10)
            return true;
        return false;
    }
    //public override bool Perform(Agent agent)
    //{
    //    // pick up the food
    //    agent.CurrentFood = 10;
    //    agent.removeCurrentState("HasFoodBasket"); // bad way of doing this, but will do for the prototype
    //    isFoodRestocked = true;
    //    return true;
    //}
    protected override bool OnComplete(Agent agent)
    {
        // pick up the food
        agent.CurrentFood = 10;
        agent.removeCurrentState("HasFoodBasket"); // bad way of doing this, but will do for the prototype
        isFoodRestocked = true;
        return true;
    }
}
