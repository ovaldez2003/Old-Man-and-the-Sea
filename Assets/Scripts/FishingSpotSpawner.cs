using UnityEngine;
using System.Collections.Generic;

public class FishingSpotSpawner : MonoBehaviour
{
    public GameObject fishingSpotPrefab;
    public Transform[] spawnPoints;   // Pre-placed candidate positions across the lake
    public int spotsToSpawn = 3;

    void Start()
    {
        List<Transform> pool = new List<Transform>(spawnPoints);
        int count = Mathf.Min(spotsToSpawn, pool.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            Instantiate(fishingSpotPrefab, pool[index].position, pool[index].rotation);
            pool.RemoveAt(index);
        }
    }
}