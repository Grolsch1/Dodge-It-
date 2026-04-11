using System.ComponentModel.Design.Serialization;
using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;

    public GameObject[] enemyPrefabs;
    //public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public int enemiesPerWave = 6;
    private int waveNumber = 0;
    public int maxWaves = 5;
    private int aliveEnemies = 0;
    private bool isSpawningWave = false;
    private bool bossSpawned = false;
    public static EnemySpawner instance;

    private void Awake()
    {
        instance = this;
    }

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
        StartCoroutine(NextWaveRoutine());
    }

    private IEnumerator NextWaveRoutine()
    {
        isSpawningWave = true;

        yield return new WaitForSeconds(1f); // small delay after last enemy dies

        waveNumber++;

        bool isBossWave = waveNumber == maxWaves;

        WavePopupUI.instance.ShowWave(waveNumber, isBossWave);

        yield return new WaitForSeconds(3f); // wait for popup to finish

        if (isBossWave)
        {
            SpawnBossWave();
            yield break;
        }

        aliveEnemies = 0;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
        }

        enemiesPerWave = Mathf.RoundToInt(enemiesPerWave + waveNumber * 1.5f);
        GameEvents.OnWaveUpdated?.Invoke(waveNumber);

        isSpawningWave = false;
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

        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex],
                        GetSpawnPosition(),
                        Quaternion.identity);

        aliveEnemies++;
    }

    public void OnEnemyKilled()
    {
        aliveEnemies--;

        if (aliveEnemies <= 0 && !isSpawningWave)
        {
            StartCoroutine(NextWaveRoutine());
        }
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

        GameObject boss = Instantiate(bossPrefab, Vector2.zero, Quaternion.identity);

        aliveEnemies = 1; // boss counts as one enemy
    }
}
