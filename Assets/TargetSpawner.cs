using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab; // The target prefab to spawn
    public Transform spawnArea;      // The area on the wall where targets can spawn
    public float minSpawnDelay = 1f; // Minimum time between spawns
    public float maxSpawnDelay = 3f; // Maximum time between spawns

    private float nextSpawnTime;
    private List<GameObject> activeTargets = new List<GameObject>();

    private void Start()
    {
        // Set the initial spawn time
        nextSpawnTime = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    private void Update()
    {
        // Check if it's time to spawn a target
        if (Time.time >= nextSpawnTime)
        {
            // Calculate a random position within the spawn area
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2),
                Random.Range(spawnArea.position.y - spawnArea.localScale.y / 2, spawnArea.position.y + spawnArea.localScale.y / 2),
                spawnArea.position.z - (float)0.4
            );


            GameObject newTarget = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
            newTarget.GetComponent<TargetMove>().SetAsNonOriginalTarget();
            activeTargets.Add(newTarget);
            //newTarget.transform.SetParent(spawnArea);

            // Set the next spawn time
            nextSpawnTime = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
        }

        for (int i = activeTargets.Count - 1; i >= 0; i--)
        {
            if (!activeTargets[i].activeSelf)
            {
                Destroy(activeTargets[i]);
                activeTargets.RemoveAt(i);
            }
        }

    }
}
