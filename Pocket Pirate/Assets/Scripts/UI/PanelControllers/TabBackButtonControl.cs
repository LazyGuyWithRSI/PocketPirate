using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabBackButtonControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
            //anim.Play("PanelSlideUp");
        }
        else
        {
            gameObject.SetActive(false);
            //anim.Play("PanelSlideDown");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
