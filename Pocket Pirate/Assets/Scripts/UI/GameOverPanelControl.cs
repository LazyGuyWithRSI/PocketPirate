using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanelControl : MonoBehaviour
{
    public float Delay = 2.0f;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //gameObject.SetActive(false);
        PubSub.RegisterListener<OnGameOver>(OnGameOverHandle);

        // disable panel on start up, some focus issue means spacebar hits the try again button...
        gameObject.SetActive(false);
    }

    public void OnGameOverHandle(object publishedEvent)
    {
        OnGameOver args = publishedEvent as OnGameOver;
        gameObject.SetActive(true);
        StartCoroutine(DelayPanelAnim(Delay));
    }

    private IEnumerator DelayPanelAnim(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        anim.Play("PanelSlideUp");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
