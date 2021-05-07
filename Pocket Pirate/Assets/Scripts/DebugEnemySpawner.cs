﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugEnemySpawner : MonoBehaviour
{
    public SpawningWaveReference[] waves;
    public Vector3Reference PlayerPosition;
    public FloatReference CurrentWave;
    public GameObject WakePrefab;
    public int StartEnemies = 5;
    public int MaxEnemies = 20;
    public float MaxSpawnDistance = 20f;
    public float MinSpawnDistance = 8f;
    public float WaveTimeLimit = 10f;

    public float Ramp = 1.4f;

    private int numEnemies;
    private int MaxEnemiesThisWave;
    private int enemiesToSpawn;

    private int nextWave;

    private bool gameIsOver = false;

    // Start is called before the first frame update
    void Start ()
    {
        PubSub.RegisterListener<OnDeathEvent>(OnDeathDeath);
        PubSub.RegisterListener<OnWaveOver>(OnWaveOverHandler);

        numEnemies = 0;
        nextWave = (int)CurrentWave.Value;
        /*
        for (int i = 0; i < MaxEnemies; i++)
        {
            SpawnEnemy();
        }
        */
        //StartCoroutine(WaveControlCoroutine());
        StartCoroutine(SpawnWaveCoroutine(Random.Range(0, waves.Length), (int)(StartEnemies * (Ramp * (int)CurrentWave.Value)), 2f));
    }

    private void Update ()
    {

    }

    public void OnGameOverEvent(object publishedEvent)
    {
        gameIsOver = true;
        nextWave = 1;
    }

    public void OnWaveOverHandler(object publishedEvent)
    {
        nextWave++;
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
                GameObject enemy = Instantiate(waves[wave].ThingsToSpawn[i].Prefab, new Vector3(PlayerPosition.Value.x + xDelta, 1, PlayerPosition.Value.z + zDelta), Quaternion.identity);
                //GameObject wake = Instantiate(WakePrefab);
                //wake.GetComponent<GenerateWake>().target = enemy.transform;
                numEnemies++;
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
        this.enemiesToSpawn = enemiesToSpawn;
        while (this.enemiesToSpawn > 0)
        {
            yield return new WaitForSeconds(interval);

            if (numEnemies >= MaxEnemies)
                continue;

            SpawnEnemy(wave);
            this.enemiesToSpawn--;
        }
    }
}
