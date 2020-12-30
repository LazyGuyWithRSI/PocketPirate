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

    private const string TryAgainBtnName = "BtnTryAgain";

    // Start is called before the first frame update
    void Start()
    {
        //YAMLPersistance.ReadYaml(@"Assets/AI/ai_state_machine_definitions.yml");

        Time.timeScale = 1.0f;

        // clear event system
        PubSub.ClearListeners();

        PubSub.RegisterListener<OnDeathEvent>(OnDeath);
        PubSub.RegisterListener<OnButtonReleasedEvent>(OnButtonPressed);

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
        //StartCoroutine(ReloadScene(3f, true));
        publishGameOver(false);
    }

    public void OnDeath (object publishedEvent)
    {
        OnDeathEvent args = publishedEvent as OnDeathEvent;
        if (args.Team == 0) // player died
        {
            // TODO send a GameOver event?
            //StartCoroutine(ReloadScene(3f, false));
            publishGameOver(true);
        }

        else if (args.Team == 1) // enemy died
        {
            Score.Value += 100;
        }
    }

    private void OnButtonPressed(object publishedEvent)
    {
        OnButtonReleasedEvent args = publishedEvent as OnButtonReleasedEvent;
        if (args.Name == TryAgainBtnName)
        {
            Time.timeScale = 1.0f;

            //SceneManager.LoadScene(1);
            PubSub.Publish<OnRequestSceneChange>(new OnRequestSceneChange() { SceneIndex = 1 });
        }
    }

    private void publishGameOver(bool died)
    {
        if (died)
            Time.timeScale = 1.0f;
        else
            Time.timeScale = 0.0f;

        StopAllCoroutines();
        PubSub.Publish<OnGameOver>(new OnGameOver() { Died = died });
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
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
