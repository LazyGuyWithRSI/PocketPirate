using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanelControl : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start ()
    {
        anim = GetComponent<Animator>();
        //gameObject.SetActive(false);
        PubSub.RegisterListener<OnPauseEvent>(OnPauseHandle);

        // disable panel on start up, some focus issue means spacebar hits the try again button...
        gameObject.SetActive(false);
    }

    public void OnPauseHandle (object publishedEvent)
    {
        OnPauseEvent args = publishedEvent as OnPauseEvent;
        if (args.Paused)
        {
            gameObject.SetActive(true);
            anim.Play("PanelSlideUp");
        }
    }

    // Update is called once per frame
    void Update ()
    {

    }
}