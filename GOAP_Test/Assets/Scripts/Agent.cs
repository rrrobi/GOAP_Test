using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public GameObject House;

    float decisionTimer = 1;

    float foodBaseTimer = 2;
    float foodTimer;
    float fuelBaseTimer = 5;
    float fuelTimer;
    float energyBaseTimer = 1.5f;
    float energyTimer;

    public int CurrentFood = 10;
    public int CurrentFuel = 10;
    public int CurrentEnergy = 20;

    // de-facto Blackboard
    ////////////////////////////////////////////////////////////////////////
    private HashSet<KeyValuePair<string, object>> currentState = new HashSet<KeyValuePair<string, object>>();
    public void addCurrentState(string key, object value)
    {
        currentState.Add(new KeyValuePair<string, object>(key, value));
    }
    public void removeCurrentState(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in currentState)
        {
            if (kvp.Key.Equals(key))
            {
                remove = kvp;
            }            
        }
        if (!default(KeyValuePair<string, object>).Equals(remove))
        {
            currentState.Remove(remove);
        }
    }
    public HashSet<KeyValuePair<string, object>> CurrentState
    {
        get
        {
            return currentState;
        }
    }
    ////////////////////////////////////////////////////////////////////////

    private HashSet<GOAPAction> availableActions = new HashSet<GOAPAction>();
    private Queue<GOAPAction> currentActions = new Queue<GOAPAction>();
    List<GOAPGoal> Goals = new List<GOAPGoal>();
    GOAPGoal currentGoal = null;

    public GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        foodTimer = foodBaseTimer;
        fuelTimer = fuelBaseTimer;

        // Create the list of avaiable goals
        Goal_Idle idle = new Goal_Idle("Idle", this);
        Goals.Add(idle);
        Goal_GetFood getFood = new Goal_GetFood("Get_Food", this);
        Goals.Add(getFood);
        Goal_GetFuel getFuel = new Goal_GetFuel("Get_Fuel", this);
        Goals.Add(getFuel);
        Goal_Rest rest = new Goal_Rest("Rest", this);
        Goals.Add(rest);

    }

    void UpdateTimers()
    {
        fuelTimer -= Time.deltaTime;
        foodTimer -= Time.deltaTime;
        energyTimer -= Time.deltaTime;
        decisionTimer -= Time.deltaTime;
        if (foodTimer < 0)
        {
            CurrentFood--;
            foodTimer = foodBaseTimer;
        }
        if (fuelTimer < 0)
        {
            CurrentFuel--;
            fuelTimer = fuelBaseTimer;
        }
        if (energyTimer < 0)
        {
            CurrentEnergy--;
            energyTimer = energyBaseTimer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimers();

        if (currentActions != null && currentActions.Count > 0)
            DoWork();

        if (decisionTimer < 0) // only look at making a new decision ever second... for now
        {
            decisionTimer = 1;
            // Update 'CurrentState'
            UpdateCurrentState();

            // Update Priorities, get highest priority
            int highestPriority = -1;
            GOAPGoal priorityGoal = null;
            for (int i = 0; i < Goals.Count; i++)
            {
                int priority = Goals[i].CalculatePriority();
                Debug.Log($"Goal: {Goals[i].GoalName}, has priority of {priority}.");
                if (priorityGoal == null ||
                    highestPriority < priority)
                {
                    priorityGoal = Goals[i];
                    highestPriority = priority;
                }
            }

            // if priorityGoal is not already current Goal, replace it
            if (priorityGoal != currentGoal)
            {
                Debug.Log($"{priorityGoal.GoalName} is now the current goal! Priority: {priorityGoal.CalculatePriority()}");
                if (currentGoal != null)
                    currentGoal.OnGoalDeactivated();
                currentGoal = priorityGoal;
                currentGoal.OnGoalActivated();

                // Find plan for new goal
                GetAvailableActions();
                currentActions.Clear();
                currentActions = GOAP_Planner.Plan(this, availableActions, currentGoal.GoalState, currentState);
            }

        }
    }

    void DoWork()
    {
        GOAPAction nextAction = currentActions.Peek();
        // Do we need to move towards site for action
        if (nextAction.RequiresInRange() && !nextAction.IsInRange())
        {
            if (currentTarget != nextAction.gameObject)
                currentTarget = nextAction.gameObject;

            // Move to Site
            MoveToTarget(nextAction);
        }
        // Else, do the action
        else
        {
            // We are close enough
            // DoWork, will work through the actions's Time-To-Complete
            // If this frames 'DoWork' completes the action, it will return true
            if (!nextAction.IsDone())
                if (nextAction.DoWork(this))
                    currentActions.Dequeue();

            // Was that the last Action in the queue?
            if (currentActions.Count == 0)
            {
                // Deactivate Goal
                currentGoal.OnGoalDeactivated();
                // Set curentGoal to null
                currentGoal = null;
            }
        }                
    }

    void MoveToTarget(GOAPAction action)// We need the action, probably don't need to pass it though...
    {
        Vector3 agentPosition = transform.position;
        Vector3 targetPosition = currentTarget.transform.position;

        transform.position = Vector3.MoveTowards(agentPosition, targetPosition, 4.5f * Time.deltaTime);

        // are we there yet? 
        float dist = Vector3.Distance(transform.position, targetPosition);
        if (dist < 0.5)
            action.SetInRange(true);
        else
            action.SetInRange(false);
    }

    void GetAvailableActions()
    {
        GOAPAction[] actionList = House.GetComponentsInChildren<GOAPAction>();
        foreach (var action in actionList)
        {
            availableActions.Add(action);
        }
    }

    void UpdateCurrentState()
    {
        // Remove states no longer applicable
        List<string> statesToRemove = new List<string>();
        foreach (var kvp in currentState)
        {
            switch (kvp.Key)
            {
                case "FoodStocked":
                    if (CurrentFood < 10)
                        statesToRemove.Add("FoodStocked");
                    break;
                case "FuelStocked":
                    if (CurrentFuel < 10)
                        statesToRemove.Add("FuelStocked");
                    break;
                case "IsRested":
                    if (CurrentEnergy < 20)
                        statesToRemove.Add("IsRested");
                    break;
                case "IsIdle":
                    // ignore it, not important for this prototype
                    break;
            }
        }
        foreach (var state in statesToRemove)
        {
            removeCurrentState(state);
        }

        // Add states we need
        if (CurrentFood >= 10)
            addCurrentState("FoodStocked", true);
        if (CurrentFuel >= 10)
            addCurrentState("FuelStocked", true);
        if (CurrentEnergy >= 20)
            addCurrentState("IsRested", true);
    }

}
