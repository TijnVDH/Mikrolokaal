using Mirror;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : NetworkBehaviour
{
    [SerializeField] public Inventory FoodInventory;
    public Soup Soup;

    public NPCSpawner spawner;
    public NPCPath path;

	private void Awake()
	{
        path.OnReachedTarget += () =>
        {
            if (!isServer) return;

            Debug.Log("Dropping food");
            RpcDropInventory();
            FoodInventory.NPCDropAll();
        };

        path.OnReachedExit += () =>
        {
            if (!isServer) return;

            RemoveFromSpawner();
            NetworkServer.Destroy(gameObject);
        };
	}

    [ClientRpc] private void RpcDropInventory() {
        GetComponent<Character>().ChangeInventorySlots(0);
    }

	public void StartMoving(Transform target, Soup soup, List<GameObject> leaveTargets)
    {
        path.StartMoving(target, leaveTargets);
    }

    public void RemoveFromSpawner()
    {
        spawner.RemoveFromList(this);
    }

    // when the enemy is destroyed
    private void OnDestroy()
    {
        
    }
}
