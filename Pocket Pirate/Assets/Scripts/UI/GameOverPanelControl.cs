using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanelControl : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //gameObject.SetActive(false);
        PubSub.RegisterListener<OnGameOver>(OnGameOverHandle);
    }

    public void OnGameOverHandle(object publishedEvent)
    {
        OnGameOver args = publishedEvent as OnGameOver;
        //gameObject.SetActive(true);
        anim.Play("GameOverPanelSlideUp");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
