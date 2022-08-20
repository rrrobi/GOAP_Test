using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
	// Preconditions - list or hashset
	// Effects - list or hashset
	private HashSet<KeyValuePair<string, object>> preconditions;
    private HashSet<KeyValuePair<string, object>> effects; 

    private bool inRange = false;

	protected float CostPerUnitDistance = 0.1f;
	protected float BaseCost = 1f;
	public float Cost = 1f;
    public GameObject target; // Not sure about GameObject...

	protected float actionTime;

    void Awake()
    {
		preconditions = new HashSet<KeyValuePair<string, object>>();
		effects = new HashSet<KeyValuePair<string, object>>();
		ResetActionTime();
	}

	public void DoReset()
	{
		inRange = false;
		target = null;
		ResetActionTime();
		_Reset();
	}
	protected abstract void _Reset();
	protected abstract void ResetActionTime();
    public abstract bool IsDone();
    public abstract bool CheckSpecificPrecondition(Agent agent);
    public abstract bool RequiresInRange();
	public abstract bool EffectsOverTime();
	protected abstract bool OnComplete(Agent agent);
	/// <summary>
	/// Returns True if work this frame completes the action, else return false
	/// </summary>
	/// <param name="agent"></param>
	/// <returns></returns>
	public bool DoWork(Agent agent)
    {
		actionTime -= Time.deltaTime;

		if (actionTime <= 0)        			
			return OnComplete(agent);
    
		return false;
    }
    public bool IsInRange() { return inRange; }
    public void SetInRange(bool inRange) { this.inRange = inRange;}
	public float GetCostWithDistance(float dist) { return Cost + (dist * CostPerUnitDistance); }
    public void AddPrecondition(string key, object value)
    {
        preconditions.Add(new KeyValuePair<string, object>(key, value));
    }
	public void removePrecondition(string key)
	{
		KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
		foreach (KeyValuePair<string, object> kvp in preconditions)
		{
			if (kvp.Key.Equals(key))
			{
				remove = kvp;
			}
			if (!default(KeyValuePair<string, object>).Equals(remove))
			{
				preconditions.Remove(remove);
			}
		}
	}

	public void addEffect(string key, object value)
	{
		effects.Add(new KeyValuePair<string, object>(key, value));
	}

	public void removeEffect(string key)
	{
		KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
		foreach (KeyValuePair<string, object> kvp in effects)
		{
			if (kvp.Key.Equals(key))
			{
				remove = kvp;
			}
			if (!default(KeyValuePair<string, object>).Equals(remove))
			{
				effects.Remove(remove);
			}
		}
	}

	public HashSet<KeyValuePair<string, object>> Preconditions
	{
		get
		{
			return preconditions;
		}
	}

	public HashSet<KeyValuePair<string, object>> Effects
	{
		get
		{
			return effects;
		}
	}


}
