using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    float foodBaseTimer = 2;
    float foodTimer;
    float fuelBaseTimer = 5;
    float fuelTimer;

    public int CurrentFood = 10;
    public int CurrentFuel = 10;
    public int Energy;

    //private HashSet<GOAPAction> availableActions;
    //private Queue<GOAPAction> currentActions;
    List<GOAPGoal> Goals = new List<GOAPGoal>();
    GOAPGoal currentGoal = null;

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

    // Update is called once per frame
    void Update()
    {
        fuelTimer -= Time.deltaTime;
        foodTimer -= Time.deltaTime;
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

        // Update Priorities, get highest priority
        int highestPriority = -1;
        GOAPGoal priorityGoal = null;
        for (int i = 0; i < Goals.Count; i++)
        {
            int priority = Goals[i].CalculatePriority();
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
            currentGoal = priorityGoal;
        }

    }
}
