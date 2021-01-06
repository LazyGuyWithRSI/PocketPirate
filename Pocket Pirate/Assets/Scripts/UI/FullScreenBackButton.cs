using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenBackButton : MonoBehaviour
{
    public bool ActivateOnPause = false;
    public bool ActivateOnPanelShown = false;

    // Start is called before the first frame update
    void Start ()
    {

        if (ActivateOnPause)
            PubSub.RegisterListener<OnPauseEvent>(OnPauseHandle);

        if (ActivateOnPanelShown)
        {
            PubSub.RegisterListener<UIPanelShownEvent>(OnUIPanelShownHandler);
            PubSub.RegisterListener<OnNoUIPanelShowingEvent>(OnNoUIPanelShowingHandler);
        }

        gameObject.SetActive(false);
    }

    public void OnPauseHandle (object publishedEvent)
    {
        OnPauseEvent args = publishedEvent as OnPauseEvent;
        if (args.Paused)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnUIPanelShownHandler(object publishedEvent)
    {
        gameObject.SetActive(true);
    }

    private void OnNoUIPanelShowingHandler (object publishedEvent)
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
    }
}
