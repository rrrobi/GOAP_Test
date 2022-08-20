using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGetFoodBasket : GOAPAction
{
    bool hasFoodBasket = false;

    // Start is called before the first frame update
    void Start()
    {
        addEffect("HasFoodBasket", true);
        BaseCost = Cost = 15f;
    }
    // Update is called once per frame
    void Update()
    {

    }

    protected override void _Reset()
    {
        hasFoodBasket = false;
    }
    protected override void ResetActionTime()
    {
        actionTime = 1;
    }
    public override bool IsDone() { return hasFoodBasket; }
    public override bool RequiresInRange() { return true; }
    public override bool CheckSpecificPrecondition(Agent agent)
    {
        // Can always do this        
        return true;
    }
    //public override bool Perform(Agent agent)
    //{
    //    // Todo... pick up the food basket
    //    agent.addCurrentState("HasFoodBasket", true);
    //    hasFoodBasket = true;
    //    return true;
    //}
    protected override bool OnComplete(Agent agent)
    {
        // Pick up the food basket
        agent.addCurrentState("HasFoodBasket", true);
        hasFoodBasket = true;
        return true;
    }
}
