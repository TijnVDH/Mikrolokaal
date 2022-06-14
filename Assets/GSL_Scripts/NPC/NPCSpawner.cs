using Mirror;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spawner))]
public class NPCSpawner : NetworkBehaviour
{
    public int HordeSize = 5;
    public float CooldownTime;
    public GameObject HordeTarget;
    public List<GameObject> LeaveTargets;
    [Range(0,1)] public float UpgradeChance;
    public Soup Soup;

    private Spawner spawner;
    private List<NPC> spawnedObjects;
    public void Start()
    {
        spawnedObjects = new List<NPC>();
        spawner = this.GetComponent<Spawner>();
        spawner.OnObjectSpawned += OnObjectSpawned;
    }

    public void OnObjectSpawned(GameObject spawnedObject)
    {
        NPC spawnedNpc = spawnedObject.GetComponent<NPC>();
        spawnedNpc.spawner = this;

		if (Random.value < UpgradeChance)
		{
            // enhance virus strength
            spawnedNpc.GetComponent<Character>().RpcUpgrade(UpgradeType.ATTACK);
		}

        spawnedObjects.Add(spawnedNpc);

        if (spawnedObjects.Count == HordeSize)
        {
            SendHorde();
            spawner.StopSpawning();
        }
    }

    public void SendHorde()
    {
        foreach (NPC npc in spawnedObjects)
        {
            npc.StartMoving(HordeTarget.transform, Soup, LeaveTargets);
        }
        spawnedObjects.Clear();
    }

    public void RemoveFromList(NPC npc)
    {
        spawnedObjects.Remove(npc);
    }
}
