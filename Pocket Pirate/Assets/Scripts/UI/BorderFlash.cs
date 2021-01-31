using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderFlash : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        PubSub.RegisterListener<OnHitEvent>(OnHitHandler);
    }

    private void OnHitHandler (object pubEvent)
    {
        OnHitEvent args = pubEvent as OnHitEvent;
        if (args.Team == 0) // player
            anim.Play("BorderFlash", -1, 0f);
    }

}
