using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSleep : GOAPAction
{
	int targetEnergy = 20; // this is a quick temp solution - would have a max value/ target value done properly, and probably in the agent class rather than here

	private float intervalBase = 0.5f;
	private float intervalTime;
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
		actionTime = 5;
		intervalTime = intervalBase;
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
		//agent.CurrentEnergy = 20;
		isRested = true;
		return true;
	}
	/// <summary>
	/// Returns True if work this frame completes the action, else return false. ONLY overridden by EffectOverTime actions
	/// </summary>
	/// <param name="agent"></param>
	/// <returns></returns>
	public override bool DoWork(Agent agent)
	{
		actionTime -= Time.deltaTime;
		intervalTime -= Time.deltaTime;

		if (intervalTime <= 0)
		{
			agent.CurrentEnergy++;
			intervalTime = intervalBase;
		}

		if (actionTime <= 0 || agent.CurrentEnergy >= targetEnergy)
			return OnComplete(agent);

		return false;
	}
}
