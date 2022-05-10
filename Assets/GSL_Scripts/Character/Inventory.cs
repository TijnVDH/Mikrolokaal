using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [Header("General")]
    public int DefaultSlotsAmount;
    public int CurrentSlotsAmount;

    [Header("Food spawning")]
    public float minDistance = 0;
    public float maxDistance = 0;
    public float targetY = 0;
    public GameObject TriangleFoodPrefab;
    public GameObject CircleFoodPrefab;
    public GameObject SquareFoodPrefab;

    private List<FoodType> items = new List<FoodType>();

    public List<FoodType> Items { get { return items; } }

    private void Start()
    {
        CurrentSlotsAmount = DefaultSlotsAmount;
    }

    public bool HasSpace()
    {
        return items.Count < CurrentSlotsAmount;
    }

    public void AddToInventory(FoodType food)
    {
        items.Add(food);
    }

    public void DropAll()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            FoodType droppedFood = items[i];
            if (isLocalPlayer)
            {
                CmdSpawnDroppedFood(droppedFood);
            }
            items.RemoveAt(i);
        }
    }

    public void NPCDropAll()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            FoodType droppedFood = items[i];
            CmdSpawnDroppedFood(droppedFood);
            items.RemoveAt(i);
        }
    }

    [Command(requiresAuthority=false)]
    public void CmdSpawnDroppedFood(FoodType food)
    {
        // Get correct prefab
        GameObject prefab;
        switch (food)
        {
            case FoodType.CIRCLE:
                prefab = CircleFoodPrefab;
                break;
            case FoodType.SQUARE:
                prefab = SquareFoodPrefab;
                break;
            case FoodType.TRIANGLE:
                prefab = TriangleFoodPrefab;
                break;
            default:
                Debug.LogWarningFormat("Can't drop unknown foodtype '{0}", food);
                return;
        }

        // randomise target position;
        Vector2 direction = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minDistance, maxDistance);
        Vector2 position = new Vector2(transform.position.x, transform.position.z) + (distance * direction);
        Vector3 targetPosition = new Vector3()
        {
            x = position.x,
            y = targetY,
            z = position.y,
        };

        // Spawn food
        GameObject spawnedObject = Instantiate(prefab, targetPosition, Quaternion.identity);

        NetworkServer.Spawn(spawnedObject);
        Debug.Log("Inventory: spawned food (" + spawnedObject.ToString() + ")");
    }

    public void ChangeSlotsAmount(int newAmount)
    {
        if (newAmount > CurrentSlotsAmount)
        {
            // More slots, simply add a new one
            CurrentSlotsAmount = newAmount;
        }
        else if (newAmount < CurrentSlotsAmount)
        {
            // remove items that are too many
            for (int i = items.Count - 1; i >= newAmount; i--)
            {
                items.RemoveAt(i);
            }

            CurrentSlotsAmount = newAmount;
        }
    }
}
