using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugEnemySpawner : MonoBehaviour
{
    public SpawningWaveReference[] waves;
    public Vector3Reference PlayerPosition;
    public int StartEnemies = 5;
    public float MaxSpawnDistance = 20f;
    public float MinSpawnDistance = 8f;
    public float WaveTimeLimit = 10f;

    public float Ramp = 1.4f;

    private int numEnemies;
    private int MaxEnemiesThisWave;

    private bool gameIsOver = false;

    // Start is called before the first frame update
    void Start ()
    {
        PubSub.RegisterListener<OnDeathEvent>(OnDeathDeath);

        numEnemies = 0;

        /*
        for (int i = 0; i < MaxEnemies; i++)
        {
            SpawnEnemy();
        }
        */
        StartCoroutine(WaveControlCoroutine());
    }

    private void FixedUpdate ()
    {
        /*
        elapsed += Time.fixedDeltaTime;

        extraEnemies = (int)(elapsed * Ramp);
        if (numEnemies < MaxEnemies + extraEnemies)
            SpawnEnemy();
        */
    }

    public void OnGameOverEvent(object publishedEvent)
    {
        gameIsOver = true;
    }

    public void OnDeathDeath (object publishedEvent) // TODO make sure this is actually an enemy perhaps?
    {
        OnDeathEvent args = publishedEvent as OnDeathEvent;
        if (args.Team == 1)
        {
            /*
            numEnemies--;
            if (numEnemies < MaxEnemies + extraEnemies)
                SpawnEnemy();
            */
        }
    }

    private void SpawnEnemy(int wave)
    {
        if (gameIsOver)
            return;

        float xDelta = 0;
        float zDelta = 0;

        while (Mathf.Abs(zDelta) < MinSpawnDistance && Mathf.Abs(xDelta) < MinSpawnDistance) // I DON'T LIKE THIS
        {
            zDelta = Random.Range(-MaxSpawnDistance, MaxSpawnDistance);
            xDelta = Random.Range(-MaxSpawnDistance, MaxSpawnDistance);
        }

        // add up weights
        
        float totalWeight = 0;
        for (int i = 0; i < waves[wave].ThingsToSpawn.Length; i++)
            totalWeight += waves[wave].ThingsToSpawn[i].Weight;

        float rand = Random.Range(0, totalWeight);
        for (int i = 0; i < waves[wave].ThingsToSpawn.Length; i++)
        {
            if (rand < waves[wave].ThingsToSpawn[i].Weight)
            {
                Instantiate(waves[wave].ThingsToSpawn[i].Prefab, new Vector3(PlayerPosition.Value.x + xDelta, 1, PlayerPosition.Value.z + zDelta), Quaternion.identity);
                break;
            }

            rand -= waves[wave].ThingsToSpawn[i].Weight;
        }

        /*
        if (Random.Range(0, 4) == 0)
            Instantiate(BigEnemyPrefab, new Vector3(PlayerPosition.Value.x + xDelta, 1, PlayerPosition.Value.z + zDelta), Quaternion.identity);
        else
            Instantiate(EnemyPrefab, new Vector3(PlayerPosition.Value.x + xDelta, 1, PlayerPosition.Value.z + zDelta), Quaternion.identity);
        numEnemies++;
        */
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
        while (enemiesToSpawn > 0)
        {
            yield return new WaitForSeconds(interval);

            SpawnEnemy(wave);
            enemiesToSpawn--;
        }
    }
}
