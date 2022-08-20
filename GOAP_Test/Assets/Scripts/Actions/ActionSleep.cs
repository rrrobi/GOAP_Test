using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSleep : GOAPAction
{
	private bool isRested = false;
	public ActionSleep()
	{
		addEffect("IsRested", true);
		BaseCost = Cost = 100f;// Cost isdetermined by..... what exactly? its a guess right now
	}

	protected override void _Reset()
    {
		isRested = false;
    }
	protected override void ResetActionTime()
	{
		actionTime = 1;
	}
	public override bool IsDone() { return isRested; }
	public override bool RequiresInRange() { return true; }
	public override bool CheckSpecificPrecondition(Agent agent)
	{
		// Todo.. Is the agent Tired?
		return false;
    }
	//public override bool Perform(Agent agent)
 //   {
	//	// Todo... Sleep in the bed
	//	isRested = true;
	//	return false;
 //   }
	protected override bool OnComplete(Agent agent)
	{
		// Sleep in the bed
		isRested = true;
		return false;
	}
}
