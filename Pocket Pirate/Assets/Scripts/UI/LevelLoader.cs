using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator Transition;
    public float TransitionTime = 1f;

    void Start()
    {
        PubSub.RegisterListener<OnRequestSceneChange>(OnRequestSceneChangeHandler);
    }

    public void OnRequestSceneChangeHandler(object obj)
    {
        OnRequestSceneChange args = obj as OnRequestSceneChange;
        StartCoroutine(Load(args.SceneIndex));

    }

    public void LoadMainGame ()
    {
        StartCoroutine(Load(1));
    }

    public void LoadMainMenu ()
    {
        StartCoroutine(Load(0));
    }

    IEnumerator Load (int index)
    {
        Transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(TransitionTime);
        SceneManager.LoadScene(index);
    }
}
