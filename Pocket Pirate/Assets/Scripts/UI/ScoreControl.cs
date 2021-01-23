using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreControl : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        PubSub.RegisterListener<OnCoinPickUpEvent>(OnCoinPickupHandler);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCoinPickupHandler (object pubEvent)
    {
        OnCoinPickUpEvent args = pubEvent as OnCoinPickUpEvent;
        anim.SetTrigger("Play");
    }
}
