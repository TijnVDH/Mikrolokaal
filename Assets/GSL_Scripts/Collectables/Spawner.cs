using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    [SerializeField] private List<GameObject> spawnOptions;
    [SerializeField] private bool startSpawningOnEnable;
    [SerializeField] private float spawnRate    = 0;
    [SerializeField] private float minDistance  = 0;
    [SerializeField] private float maxDistance  = 0;
    [SerializeField] private float targetY      = 0;

    public delegate void OnObjectSpawnedEvent(GameObject spawnedObject);
    public event OnObjectSpawnedEvent OnObjectSpawned;

    private Coroutine SpawnCoroutine;
    private bool cooldown;

	private void OnEnable()
	{
		if (startSpawningOnEnable) StartSpawning();
	}

	public void StartSpawning()
	{
        SpawnCoroutine = StartCoroutine(AutoSpawn());
    }

    public void StopSpawning()
	{
        if (SpawnCoroutine != null)
        {
            StopCoroutine(SpawnCoroutine);
            SpawnCoroutine = null;
        }
    }

    public void Spawn()
	{
        if(cooldown == false)
        {
            // select and instantiate object
            GameObject spawnPrefab = spawnOptions[Random.Range(0, spawnOptions.Count)];

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

            GameObject spawnedObject = Instantiate(spawnPrefab, targetPosition, Quaternion.identity);

            NetworkServer.Spawn(spawnedObject);
            OnObjectSpawned?.Invoke(spawnedObject);

            Invoke("ResetCooldown", 3.0f);
            cooldown = true;
        }
    }

    void ResetCooldown()
    {
        cooldown = false;
    }

    private IEnumerator AutoSpawn()
	{
		while (true)
		{
            yield return new WaitForSeconds(spawnRate);
            Spawn();
        }
	}
}
