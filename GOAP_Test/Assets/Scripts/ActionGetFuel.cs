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
    }
    // Update is called once per frame
    void Update()
    {

    }

    protected override void _Reset()
    {
        isFuelRestocked = false;
    }
    public override bool IsDone() { return isFuelRestocked; }
    public override bool RequiresInRange() { return true; }
    public override bool CheckSpecificPrecondition(Agent agent)
    {
        // Can we collect more fuel?
        if (agent.CurrentFuel < 10)
            return true;
        return false;
    }
    public override bool Perform(Agent agent)
    {
        // Todo... pick up the fuel
        agent.CurrentFuel = 10;
        isFuelRestocked = true;
        return true;
    }
}
