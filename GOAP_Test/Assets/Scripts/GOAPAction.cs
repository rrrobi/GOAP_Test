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

    public float Cost = 1f;
    public GameObject target; // Not sure about GameObject...

    void Awake()
    {
		preconditions = new HashSet<KeyValuePair<string, object>>();
		effects = new HashSet<KeyValuePair<string, object>>();
	}

	public void doReset()
	{
		inRange = false;
		target = null;
		_Reset();
	}
	public abstract void _Reset();
    public abstract bool IsDone();
    public abstract bool CheckSpecificPrecondition(Agent agent);
    public abstract bool Perform(Agent agent);
    public abstract bool RequiresInRange();
    public bool IsInRange() { return inRange; }
    public void SetInRange(bool inRange) { this.inRange = inRange;}
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
