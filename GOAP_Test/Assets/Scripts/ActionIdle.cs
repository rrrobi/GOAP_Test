using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIdle : GOAPAction
{
    bool isIdle = false;
    // Start is called before the first frame update
    void Start()
    {
        addEffect("IsIdle", true);
        BaseCost = Cost = 10f;
    }
    // Update is called once per frame
    void Update()
    {

    }

    protected override void _Reset()
    {
        isIdle = false;
    }
    public override bool IsDone() { return isIdle; }
    public override bool RequiresInRange() { return true; }
    public override bool CheckSpecificPrecondition(Agent agent)
    {
        // why would we not be able to idle, so always return true
        return true;
    }
    public override bool Perform(Agent agent)
    {
        isIdle = true;
        return true;
    }
}
