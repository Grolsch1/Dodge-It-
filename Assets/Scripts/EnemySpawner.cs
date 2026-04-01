using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;

    public GameObject[] enemyPrefabs;
    //public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public int enemiesPerWave = 6;

    private int waveNumber = 0;
    public int maxWaves = 5;

    private bool bossSpawned = false;

    private void OnEnable()
    {
        GameEvents.OnGameStart += StartSpawning;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= StartSpawning;
    }

    void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnWave), 2f, timeBetweenWaves);
    }

    void SpawnWave()
    {
        waveNumber++;

        Debug.Log("Wave: " + waveNumber);

        bool isBossWave = waveNumber == maxWaves;

        WavePopupUI.instance.ShowWave(waveNumber, isBossWave);

        if (isBossWave)
        {
            SpawnBossWave();
            CancelInvoke();
            return;
        }
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
        }

        enemiesPerWave = Mathf.RoundToInt(enemiesPerWave + waveNumber * 1.5f);
        GameEvents.OnWaveUpdated?.Invoke(waveNumber);
    }

    Vector2 GetSpawnPosition()
    {
        Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        float spawnDistance = 10f;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        return playerPos + randomDirection * spawnDistance;
    }

    void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        //int spawnIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(enemyPrefabs[enemyIndex],
                    GetSpawnPosition(),
                    Quaternion.identity);
    }

    void SpawnBossWave()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        if (bossSpawned) return;

        bossSpawned = true;

        Debug.Log("Boss Wave");

        Vector2 spawnPos = Vector2.zero;

        Instantiate(bossPrefab, spawnPos, Quaternion.identity);
    }
}
