using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelControl : MonoBehaviour, UIPanel
{
    Animator anim;

    // Start is called before the first frame update
    void Start ()
    {
        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
        PubSub.RegisterListener<ShowUIPanelEvent>(ShowUIPanelHandler);
    }

    public void ShowUIPanelHandler(object publishedEvent)
    {
        ShowUIPanelEvent args = publishedEvent as ShowUIPanelEvent;

        if (args.PanelName.Equals(gameObject.name))
        {
            Show();
            PubSub.Publish<UIPanelShownEvent>(new UIPanelShownEvent() { panel = this });
        }
    }

    public void Hide ()
    {
        anim.Play("PanelSlideDown");
    }

    public void Show ()
    {
        gameObject.SetActive(true);
        anim.Play("PanelSlideUp");
    }
}

public interface UIPanel
{
    void Show ();
    void Hide ();
}