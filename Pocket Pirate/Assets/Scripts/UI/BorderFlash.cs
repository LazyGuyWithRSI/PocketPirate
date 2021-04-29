using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BorderFlash : MonoBehaviour
{
    public Color DamageColor = Color.red;
    public Color HealColor = Color.green;

    private Animator anim;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        image = GetComponentInChildren<Image>();
        PubSub.RegisterListener<OnHitEvent>(OnHitHandler);
        PubSub.RegisterListener<OnPlayerHealed>(OnPlayerHealedHandler);
    }

    private void OnHitHandler (object pubEvent)
    {
        OnHitEvent args = pubEvent as OnHitEvent;
        if (args.Team == 0) // player
        {
            image.color = DamageColor;
            anim.Play("BorderFlash", -1, 0f);
        }
    }

    private void OnPlayerHealedHandler(object pubEvent)
    {
        image.color = HealColor;
        anim.Play("BorderFlash", -1, 0f);
    }

}
