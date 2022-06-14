using Mirror;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class NPC : NetworkBehaviour
{
    [SerializeField] public Inventory FoodInventory;
    public Soup Soup;

    public NPCSpawner spawner;
    public NPCPath path;
    [SerializeField] GameObject animator;
    private void Awake()
    {
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));

        path.OnReachedTarget += () =>
        {
            if (!isServer) return;
            Debug.Log("Dropping food");
            animator.GetComponent<Animator>().Play("DropItem");
            StartCoroutine(DropInventory());
        };

        path.OnReachedExit += () =>
        {
            if (!isServer) return;

            RemoveFromSpawner();
            NetworkServer.Destroy(gameObject);
        };
    }

    IEnumerator DropInventory()
    {
        yield return new WaitForSeconds(2);
        RpcDropInventory();
        FoodInventory.NPCDropAll();
    }

    [ClientRpc]
    private void RpcDropInventory()
    {
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
}
