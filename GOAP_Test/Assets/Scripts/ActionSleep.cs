using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSleep : GOAPAction
{
	private bool isRested = false;
	public ActionSleep()
	{
		addEffect("IsRested", true);
		Cost = 100f;// Cost isdetermined by..... what exactly? its a guess right now
	}

	public override void _Reset()
    {
		isRested = false;
    }
	public override bool IsDone() { return isRested; }
	public override bool RequiresInRange() { return true; }
	public override bool CheckPrecondition(GameObject Agent)
    {
		// Todo.. Is the agent Tired?
		return false;
    }
	public override bool Perform(GameObject Agent)
    {
		// Todo... Sleep in the bed
		return false;
    }
	
}
