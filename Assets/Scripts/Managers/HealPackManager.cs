using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HealPackManager : MonoBehaviour
{
    public static HealPackManager instance;

    [SerializeField] private GameObject healPackPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int maxActivePacks = 2;

    private List<GameObject> activePacks = new List<GameObject>();
    private List<Transform> availableSpawnPoints = new List<Transform>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        availableSpawnPoints.AddRange(spawnPoints);

        for (int i = 0; i < maxActivePacks; i++)
        {
            SpawnHealPack();
        }
    }

    void SpawnHealPack()
    {
        if (availableSpawnPoints.Count == 0) return;

        int index = Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[index];

        GameObject pack = Instantiate(healPackPrefab, spawnPoint.position, Quaternion.identity);

        activePacks.Add(pack);
        availableSpawnPoints.RemoveAt(index);
    }

    public void OnHealPackCollected(HealPack pack)
    {
        GameObject packObj = pack.gameObject;

        activePacks.Remove(packObj);

        Transform closest = GetClosestSpawnPoint(pack.transform.position);
        if (closest != null && !availableSpawnPoints.Contains(closest))
        {
            availableSpawnPoints.Add(closest);
        }

        Destroy(packObj);
        SpawnHealPack();
    }

    Transform GetClosestSpawnPoint(Vector3 pos)
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (Transform t in spawnPoints)
        {
            float dist = Vector3.Distance(pos, t.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }

        return closest;
    }
}
