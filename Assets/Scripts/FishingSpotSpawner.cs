using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishingSpotSpawner : MonoBehaviour
{
    public static FishingSpotSpawner Instance { get; private set; }

    public GameObject fishingSpotPrefab;
    public Transform[] spawnPoints;
    public int spotsToSpawn = 3;
    public float respawnDelay = 5f;

    private List<Transform> availablePoints;
    private Dictionary<GameObject, Transform> spotToPoint = new Dictionary<GameObject, Transform>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        availablePoints = new List<Transform>(spawnPoints);
        int count = Mathf.Min(spotsToSpawn, availablePoints.Count);
        for (int i = 0; i < count; i++)
            SpawnAtRandomPoint();
    }

    void SpawnAtRandomPoint()
    {
        if (availablePoints.Count == 0) return;

        int index = Random.Range(0, availablePoints.Count);
        Transform point = availablePoints[index];
        availablePoints.RemoveAt(index);

        GameObject spot = Instantiate(fishingSpotPrefab, point.position, point.rotation);
        spotToPoint[spot] = point;
    }

    public void NotifySpotFinished(GameObject spot)
    {
        if (spotToPoint.TryGetValue(spot, out Transform point))
        {
            availablePoints.Add(point);
            spotToPoint.Remove(spot);
        }

        Destroy(spot);
        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnAtRandomPoint();
    }
}