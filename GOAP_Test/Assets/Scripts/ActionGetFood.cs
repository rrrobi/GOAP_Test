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
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void _Reset()
    {
        isFoodRestocked = false;
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
    public override bool Perform(Agent agent)
    {
        // Todo... Sleep in the bed
        agent.CurrentFood = 10;
        isFoodRestocked = true;
        return true;
    }
}
