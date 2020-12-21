using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float TotalGameTime = 20f; // TODO move to scriptable (for data driven woot)

    public FloatReference GameTime;
    public FloatReference Score;
    // TODO add GameState

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        // clear event system
        PubSub.ClearListeners();

        PubSub.RegisterListener<OnDeathEvent>(OnDeath);

        Score.Value = 0;

        // start game timer
        StartCoroutine(gameTimeCountdown());
    }

    private IEnumerator gameTimeCountdown()
    {
        float time = TotalGameTime;
        while (time >= 0)
        {
            GameTime.Value = (int)Mathf.Round(time);
            time -= Time.deltaTime;
            yield return null;
        }

        // TODO end game
        StartCoroutine(ReloadScene(3f, true));
    }

    public void OnDeath (object publishedEvent)
    {
        OnDeathEvent args = publishedEvent as OnDeathEvent;
        if (args.Team == 0) // player died
        {
            // TODO send a GameOver event?
            StartCoroutine(ReloadScene(3f, false));
        }

        else if (args.Team == 1) // enemy died
        {
            Score.Value += 100;
        }
    }

    private IEnumerator ReloadScene (float delay, bool pauseGame)
    {
        if (pauseGame)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;

        Debug.Log("routine running..");
        yield return new WaitForSecondsRealtime(delay);

        Debug.Log("changing scene");
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
