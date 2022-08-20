using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGetFuel : GOAPAction
{
    bool isFuelRestocked = false;

    // Start is called before the first frame update
    void Start()
    {
        addEffect("FuelStocked", true);
        AddPrecondition("HasFuelSack", true);
        BaseCost = Cost = 15f;
    }
    // Update is called once per frame
    void Update()
    {

    }

    protected override void _Reset()
    {
        isFuelRestocked = false;
    }
    protected override void ResetActionTime()
    {
        actionTime = 1;
    }
    public override bool IsDone() { return isFuelRestocked; }
    public override bool RequiresInRange() { return true; }
    public override bool EffectsOverTime() { return false; }
    public override bool CheckSpecificPrecondition(Agent agent)
    {
        // Can we collect more fuel?
        if (agent.CurrentFuel < 10)
            return true;
        return false;
    }
    protected override bool OnComplete(Agent agent)
    {
        // pick up the fuel
        agent.CurrentFuel = 10;
        agent.removeCurrentState("HasFuelSack"); // bad way of doing this, but will do for the prototype
        isFuelRestocked = true;
        return true;
    }
}
