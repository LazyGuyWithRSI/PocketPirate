using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugEnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject BigEnemyPrefab;
    public Vector3Reference PlayerPosition;
    public int MaxEnemies = 5;
    public float MaxSpawnDistance = 20f;
    public float MinSpawnDistance = 8f;

    public float Ramp = 0.5f;

    private int numEnemies;
    private float elapsed;
    private int extraEnemies = 0;

    private bool gameIsOver = false;

    // Start is called before the first frame update
    void Start ()
    {
        PubSub.RegisterListener<OnDeathEvent>(OnDeathDeath);

        numEnemies = 0;

        for (int i = 0; i < MaxEnemies; i++)
        {
            SpawnEnemy();
        }

        elapsed = 0f;
    }

    private void FixedUpdate ()
    {
        elapsed += Time.fixedDeltaTime;

        extraEnemies = (int)(elapsed * Ramp);
        if (numEnemies < MaxEnemies + extraEnemies)
            SpawnEnemy();
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
            numEnemies--;
            if (numEnemies < MaxEnemies + extraEnemies)
                SpawnEnemy();
        }
    }

    private void SpawnEnemy()
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

        if (Random.Range(0, 4) == 0)
            Instantiate(BigEnemyPrefab, new Vector3(PlayerPosition.Value.x + xDelta, 1, PlayerPosition.Value.z + zDelta), Quaternion.identity);
        else
            Instantiate(EnemyPrefab, new Vector3(PlayerPosition.Value.x + xDelta, 1, PlayerPosition.Value.z + zDelta), Quaternion.identity);

        numEnemies++;
    }


}
