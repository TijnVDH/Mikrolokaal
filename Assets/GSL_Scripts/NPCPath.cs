using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AIDestinationSetter))]
public class NPCPath : AIPath
{
    private AIDestinationSetter destinationSetter;
    private List<GameObject> leaveTargets;

    private bool firstTargetReached = false;

    public event PathAction OnReachedExit;
    public event PathAction OnReachedTarget;
    public delegate void PathAction();

    public void StartMoving(Transform target, List<GameObject> leaveTargets)
    {
        this.leaveTargets = leaveTargets;
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = target;
        canSearch = true;
        canMove = true;
    }

    public override void OnTargetReached()
    {
        if (firstTargetReached == false)
            firstTargetReached = true;
        else
        {
            OnReachedExit();
        }

        Debug.Log("target reached");
        base.OnTargetReached();

        OnReachedTarget();

        //if (FoodInventory.Count > 0)
        //{
        //    Debug.Log("got food, dropping it");
        //    FoodInventory.DropAll();
        //}
        //else
        //{
        //    //         Debug.Log("got no food, removing from soup");
        //    //         Array enumValues = Enum.GetValues(typeof(FoodType));
        //    //         System.Random fran = new System.Random();
        //    //         FoodType randomFood = (FoodType)enumValues.GetValue(fran.Next(enumValues.Length));

        //    //         Debug.Log("removing!");
        //    //this.Soup.RemoveFood(randomFood, 1);
        //}

        System.Random tran = new System.Random();
        destinationSetter.target = leaveTargets[tran.Next(leaveTargets.Count)].transform;
    }

    
}
