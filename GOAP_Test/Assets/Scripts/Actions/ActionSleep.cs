using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSleep : GOAPAction
{
	private bool isRested = false;
	void Start()
	{
		addEffect("IsRested", true);
		BaseCost = Cost = 10f;// Cost isdetermined by..... what exactly? its a guess right now
	}
	void Update()
	{

	}

	protected override void _Reset()
    {
		isRested = false;
    }
	protected override void ResetActionTime()
	{
		actionTime = 2;
	}
	public override bool IsDone() { return isRested; }
	public override bool RequiresInRange() { return true; }
	public override bool EffectsOverTime() { return false; }
	public override bool CheckSpecificPrecondition(Agent agent)
	{
		// can always do this
		return true;
    }
	protected override bool OnComplete(Agent agent)
	{
		// pick up the food
		agent.CurrentEnergy = 20;
		isRested = true;
		return true;
	}
}
