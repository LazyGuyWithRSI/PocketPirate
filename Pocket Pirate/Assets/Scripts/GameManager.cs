using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float TotalGameTime = 20f; // TODO move to scriptable (for data driven woot)
    public float WaveOverDelay = 5.0f;
    public bool Playing = false;

    public FloatReference GameTime;
    public FloatReference Score;
    public FloatReference TotalScore;
    public FloatReference currentWave;
    public FloatReference nextWave;
    public FloatReference portSkipWaveBonus;
    public BoolReference GameIsPaused;
    public BoolReference IsAPanelShowing;
    // TODO add GameState

    private const string TryAgainBtnName = "BtnTryAgain";
    private const string PauseBtnName = "BtnPause";
    private const string TapBackBtnName = "TapBtnBack";
    private const string BtnMainMenu = "BtnMainMenu";
    private const string BtnResumeName = "BtnResume";
    private const string BtnNextWaveName = "BtnNextWave";

    private bool unPausing = false;
    private bool waveOverFired = false;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        GameIsPaused.Value = false;
        unPausing = false;

        // clear event system
        PubSub.ClearListeners();

        PubSub.RegisterListener<OnDeathEvent>(OnDeath);
        PubSub.RegisterListener<OnWaveOver>(OnWaveOverHandler);
        PubSub.RegisterListener<OnButtonReleasedEvent>(OnButtonPressed);
        PubSub.RegisterListener<OnCoinPickUpEvent>(OnCoinPickUpHandler);
        PubSub.RegisterListener<OnReset>(OnResetHandler);


        if (Playing)
        {
            //Score.Value = 0;
            StartCoroutine(gameTimeCountdown());
            currentWave.Value = nextWave.Value;
        }
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
        if (!waveOverFired)
            PubSub.Publish<OnWaveOver>(new OnWaveOver() { Imediate = true });
        //publishGameOver(false);
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
    }

    private void OnCoinPickUpHandler(object pubEvent)
    {
        OnCoinPickUpEvent args = pubEvent as OnCoinPickUpEvent;
        Score.Value += args.Worth;
        TotalScore.Value += args.Worth;
    }

    private void OnWaveOverHandler(object pubEvent)
    {
        OnWaveOver args = pubEvent as OnWaveOver;

        waveOverFired = true;
        nextWave.Value = currentWave.Value + 1;

        if (args.Imediate)
            StartCoroutine(DelayWaveOver(0f));
        else
            StartCoroutine(DelayWaveOver(WaveOverDelay));
    }

    private void OnButtonPressed(object publishedEvent)
    {
        OnButtonReleasedEvent args = publishedEvent as OnButtonReleasedEvent;

        if (args.Name == TryAgainBtnName)
        {
            Time.timeScale = 1.0f;
            //SceneManager.LoadScene(1);
            PubSub.Publish<OnReset>(new OnReset());
            PubSub.Publish<OnRequestSceneChange>(new OnRequestSceneChange() { SceneIndex = 1 });
        }
        else if (args.Name == PauseBtnName && !unPausing)
        {
            GameIsPaused.Value = true;
            Time.timeScale = 0.0f;
            PubSub.Publish<OnPauseEvent>(new OnPauseEvent() { Paused = true });
            PubSub.Publish<OnShowPausePanel>(new OnShowPausePanel() { Show = true });
        }
        else if (GameIsPaused.Value && args.Name == BtnResumeName)
        {
            Unpause();
        }
        else if (args.Name == TapBackBtnName)
        {
            if (!IsAPanelShowing.Value)
            {
                Unpause();
            }
        }
        else if (args.Name == BtnNextWaveName)
        {
            //Time.timeScale = 1.0f;
            //SceneManager.LoadScene(1);
            Score.Value += portSkipWaveBonus.Value;
            PubSub.Publish<OnRequestSceneChange>(new OnRequestSceneChange() { SceneIndex = 1 });
        }
        else if (args.Name == BtnMainMenu)
        {
            //Time.timeScale = 1.0f;
            //SceneManager.LoadScene(1);
            if (isGameOver)
                SOPersistance.Instance.ResetAll();
            PubSub.Publish<OnRequestSceneChange>(new OnRequestSceneChange() { SceneIndex = 0 });
        }
    }

    private void OnResetHandler(object publishedEvent)
    {
        Debug.Log("reseting");
        SOPersistance.Instance.ResetAll();
    }

    private void publishGameOver(bool died)
    {
        if (died)
            Time.timeScale = 1.0f;
        else
            Time.timeScale = 0.0f;

        isGameOver = true;
        StopAllCoroutines();
        PubSub.Publish<OnGameOver>(new OnGameOver() { Died = died });
    }

    private void Unpause()
    {
        GameIsPaused.Value = false;
        PubSub.Publish<OnPauseEvent>(new OnPauseEvent() { Paused = false });
        PubSub.Publish<OnShowPausePanel>(new OnShowPausePanel() { Show = false });
        StartCoroutine(DelayUnpause(1.5f));
    }

    private IEnumerator DelayUnpause (float delay)
    {
        unPausing = true;
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1.0f;
        unPausing = false;
    }

    private IEnumerator DelayWaveOver(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        GameIsPaused.Value = true;
        Time.timeScale = 0.0f;
        PubSub.Publish<OnPauseEvent>(new OnPauseEvent() { Paused = true });
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
