using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugEnemySpawner : MonoBehaviour
{
    public SpawningWaveReference[] waves;
    public Vector3Reference PlayerPosition;
    public FloatReference CurrentWave;
    public GameObject WakePrefab;
    public WaveTableReference WaveTable;

    public static List<Health> Enemies;

    public int StartEnemies = 5;
    public int MaxEnemies = 20;
    public float MaxSpawnDistance = 20f;
    public float MinSpawnDistance = 8f;
    public float totalSpawnDistance = 40f;
    public float WaveTimeLimit = 10f;

    public float Ramp = 1.4f;

    private int numEnemies;
    private int MaxEnemiesThisWave;
    private int enemiesToSpawn;

    private (float weight, GameObject thingToSpawn, int SpawnLimit, int totalSpawned)[] currentSpawnPool;


    private bool gameIsOver = false;

    void Awake()
    {
        Enemies = new List<Health>();
    }

    // Start is called before the first frame update
    void Start ()
    {
        PubSub.RegisterListener<OnDeathEvent>(OnDeathDeath);
        PubSub.RegisterListener<OnWaveOver>(OnWaveOverHandler);

        numEnemies = 0;
        /*
        for (int i = 0; i < MaxEnemies; i++)
        {
            SpawnEnemy();
        }
        */
        //StartCoroutine(WaveControlCoroutine());
        WaveTable.UpdateWavePool((int)CurrentWave.Value);

        Debug.Log("enemies to start: " + (int)(StartEnemies * (Ramp * (int)CurrentWave.Value)));
        StartCoroutine(SpawnWaveCoroutine(Random.Range(0, waves.Length), (int)(StartEnemies * (Ramp * (int)CurrentWave.Value)), 1.3f));
    }

    private void Update ()
    {

    }

    public void OnGameOverEvent(object publishedEvent)
    {
        gameIsOver = true;
    }

    public void OnWaveOverHandler(object publishedEvent)
    {

    }

    public void OnDeathDeath (object publishedEvent) // TODO make sure this is actually an enemy perhaps?
    {
        OnDeathEvent args = publishedEvent as OnDeathEvent;
        if (args.Team == 1)
        {
            numEnemies--;
            Debug.Log("Enemy died, numEnemies is " + numEnemies + ", enemiesToSpawn is " + enemiesToSpawn);
            if (numEnemies == 0 && enemiesToSpawn == 0)
                PubSub.Publish<OnWaveOver>(new OnWaveOver());
        }
    }

    private bool SpawnEnemy(int wave)
    {
        if (gameIsOver)
            return false;

        float xDelta = 0;
        float zDelta = 0;
        Vector3 spawnPoint = Vector3.zero;
        for (int i = 0; i <= 20; i++)
        {
            zDelta = Random.Range(-MaxSpawnDistance, MaxSpawnDistance);
            xDelta = Random.Range(-MaxSpawnDistance, MaxSpawnDistance);
            spawnPoint = new Vector3(PlayerPosition.Value.x + xDelta, 1, PlayerPosition.Value.z + zDelta);
            if (SpawnPointIsValid(spawnPoint))
                break;
        }

        if (!SpawnPointIsValid(spawnPoint))
        {
            Debug.Log("Could not find place to spawn");
            return false;
        }

        // add up weights
        bool isSomethingToSpawn = false;
        float totalWeight = 0;
        for (int i = 0; i < currentSpawnPool.Length; i++)
        {
            totalWeight += currentSpawnPool[i].weight;
            if (currentSpawnPool[i].totalSpawned < currentSpawnPool[i].SpawnLimit)
                isSomethingToSpawn = true;
        }

        if (!isSomethingToSpawn)
        {
            this.enemiesToSpawn = 0;
            return false;
        }

        float rand = Random.Range(0, totalWeight);
        for (int i = 0; i < currentSpawnPool.Length; i++)
        {
            if (rand <= currentSpawnPool[i].weight && currentSpawnPool[i].totalSpawned < currentSpawnPool[i].SpawnLimit)
            {
                GameObject enemy = Instantiate(currentSpawnPool[i].thingToSpawn, spawnPoint, Quaternion.identity);
                //GameObject wake = Instantiate(WakePrefab);
                //wake.GetComponent<GenerateWake>().target = enemy.transform;
                Enemies.Add(enemy.GetComponent<Health>());
                currentSpawnPool[i].totalSpawned++;

                numEnemies++;
                break;
            }

            rand -= currentSpawnPool[i].weight;
        }

        /*
        float totalWeight = 0;
        for (int i = 0; i < waves[wave].ThingsToSpawn.Length; i++)
            totalWeight += waves[wave].ThingsToSpawn[i].Weight;

        float rand = Random.Range(0, totalWeight);
        for (int i = 0; i < waves[wave].ThingsToSpawn.Length; i++)
        {
            if (rand <= waves[wave].ThingsToSpawn[i].Weight)
            {
                GameObject enemy = Instantiate(waves[wave].ThingsToSpawn[i].Prefab, spawnPoint, Quaternion.identity);
                //GameObject wake = Instantiate(WakePrefab);
                //wake.GetComponent<GenerateWake>().target = enemy.transform;
                numEnemies++;
                break;
            }

            rand -= waves[wave].ThingsToSpawn[i].Weight;
        }
        */

        return true;
    }

    private bool SpawnPointIsValid(Vector3 point)
    {
        float distanceSqr = (point - PlayerPosition.Value).sqrMagnitude;
        if (distanceSqr < Mathf.Pow(MinSpawnDistance, 2) || distanceSqr > Mathf.Pow(MaxSpawnDistance, 2))
            return false;

        if (point.sqrMagnitude > Mathf.Pow(totalSpawnDistance, 2))
            return false;

        return true;
    }

    private IEnumerator WaveControlCoroutine()
    {
        int waveCount = 0;
        StartCoroutine(SpawnWaveCoroutine(waveCount % waves.Length, StartEnemies, 1f));
        while (!gameIsOver)
        {
            yield return new WaitForSeconds(WaveTimeLimit);
            waveCount++;
            StartCoroutine(SpawnWaveCoroutine(waveCount % waves.Length, (int)(StartEnemies * (Ramp * waveCount)), 1f));
        }
    }

    private IEnumerator SpawnWaveCoroutine(int wave, int enemiesToSpawn, float interval)
    {
        this.enemiesToSpawn = enemiesToSpawn;
        List<(float weight, GameObject thingToSpawn, int SpawnLimit, int totalSpawned)> thingsToSpawnList = new List<(float weight, GameObject thingToSpawn, int SpawnLimit, int totalSpawned)>();
        foreach (WaveGroup group in WaveTable.CurrentWavePool)
            foreach (EnemySpawnGameObject enemy in group.EnemyGameObjects)
                thingsToSpawnList.Add((enemy.Weight, enemy.GameObject, enemy.SpawnLimit, 0));

        currentSpawnPool = thingsToSpawnList.ToArray();
        Debug.Log("Current spawn pool size: " + currentSpawnPool.Length);
        while (this.enemiesToSpawn > 0)
        {
            yield return new WaitForSeconds(interval);

            if (numEnemies >= MaxEnemies)
                continue;

            if (SpawnEnemy(wave))
                this.enemiesToSpawn--;
        }
    }
}
