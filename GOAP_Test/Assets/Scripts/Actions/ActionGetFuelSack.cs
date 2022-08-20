using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGetFuelSack : GOAPAction
{
    bool hasFuelSack = false;

    // Start is called before the first frame update
    void Start()
    {
        addEffect("HasFuelSack", true);
        BaseCost = Cost = 15f;
    }
    // Update is called once per frame
    void Update()
    {

    }

    protected override void _Reset()
    {
        hasFuelSack = false;
    }
    protected override void ResetActionTime()
    {
        actionTime = 1;
    }
    public override bool IsDone() { return hasFuelSack; }
    public override bool RequiresInRange() { return true; }
    public override bool EffectsOverTime() { return false; }
    public override bool CheckSpecificPrecondition(Agent agent)
    {
        // Can always do this        
        return true;
    }
    protected override bool OnComplete(Agent agent)
    {
        // Pick up the food basket
        agent.addCurrentState("HasFuelSack", true);
        hasFuelSack = true;
        return true;
    }
}
