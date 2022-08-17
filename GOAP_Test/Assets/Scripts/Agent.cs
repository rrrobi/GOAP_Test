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

    public int CurrentFood = 10;
    public int CurrentFuel = 10;
    public int Energy;

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
        //Goal_Rest rest = new Goal_Rest("Rest", this);
        //Goals.Add(rest);

    }

    void UpdateTimers()
    {
        fuelTimer -= Time.deltaTime;
        foodTimer -= Time.deltaTime;
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
            bool hasSetNewGoal = false;
            // if priorityGoal is not already current Goal, replace it
            if (priorityGoal != currentGoal)
            {
                Debug.Log($"{priorityGoal.GoalName} is now the current goal! Priority: {priorityGoal.CalculatePriority()}");
                if (currentGoal != null)
                    currentGoal.OnGoalDeactivated();
                currentGoal = priorityGoal;
                currentGoal.OnGoalActivated();
                hasSetNewGoal = true;

                // Find plan for new goal
                GetAvailableActions();
                currentActions.Clear();
                GOAPAction action = GOAP_Planner.Plan(this, availableActions, currentGoal.GoalState);
                if (action != null)
                    currentActions.Enqueue(action);
            }

            //if (!hasSetNewGoal)
            //{
            //    // goal was set last frame, goal is done, 
            //    // reset current goal to null
            //    currentGoal.OnGoalDeactivated();
            //    currentGoal = null;
            //}
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
            nextAction = currentActions.Dequeue();
            if (!nextAction.IsDone())
                nextAction.Perform(this);

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

}
