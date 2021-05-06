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
        PubSub.RegisterListener<OnShowPausePanel>(OnShowPausePanelHandle);

        // disable panel on start up, some focus issue means spacebar hits the try again button...
        gameObject.SetActive(false);
    }

    public void OnShowPausePanelHandle(object publishedEvent)
    {
        OnShowPausePanel args = publishedEvent as OnShowPausePanel;
        if (args.Show)
        {
            gameObject.SetActive(true);
            anim.Play("PanelSlideUp");
        }
        else
        {
            anim.Play("PanelSlideDown");
        }
    }

    // Update is called once per frame
    void Update ()
    {

    }
}