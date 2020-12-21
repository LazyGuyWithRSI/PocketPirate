using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugEnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Vector3Reference PlayerPosition;
    public int MaxEnemies = 5;
    public float MaxSpawnDistance = 20f;
    public float MinSpawnDistance = 8f;

    private int numEnemies;


    // Start is called before the first frame update
    void Start()
    {
        PubSub.RegisterListener<OnDeathEvent>(OnDeathDeath);

        numEnemies = 0;

        for (int i = 0; i < MaxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    public void OnDeathDeath (object publishedEvent) // TODO make sure this is actually an enemy perhaps?
    {
        OnDeathEvent args = publishedEvent as OnDeathEvent;
        if (args.Team == 1)
        {
            numEnemies--;
            if (numEnemies < MaxEnemies)
                SpawnEnemy();
        }
        else if (args.Team == 0) // player died TODO do this elsewhere
        {
            Invoke("Restart", 2);
        }
    }

    public void Restart() // TODO MOVE THIS
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SpawnEnemy()
    {
        float xDelta = 0;
        float zDelta = 0;

        while (Mathf.Abs(zDelta) < MinSpawnDistance && Mathf.Abs(xDelta) < MinSpawnDistance) // I DON'T LIKE THIS
        {
            zDelta = Random.Range(-MaxSpawnDistance, MaxSpawnDistance);
            xDelta = Random.Range(-MaxSpawnDistance, MaxSpawnDistance);
        }

        Instantiate(EnemyPrefab, new Vector3(PlayerPosition.Value.x + xDelta, 1, PlayerPosition.Value.z + zDelta), Quaternion.identity);
        numEnemies++;
    }


}
