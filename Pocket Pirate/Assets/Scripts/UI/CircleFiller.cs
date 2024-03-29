﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleFiller : MonoBehaviour
{
    private Image image;
    private bool isFilling = false;
    private float interval = 0;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        PubSub.RegisterListener<OnPlayerJump>(OnPlayerJumpHandler);
    }

    void Update ()
    {
        if (isFilling)
        {
            image.fillAmount = Mathf.MoveTowards(image.fillAmount, 1, interval * Time.deltaTime);
            if (image.fillAmount >= 1)
                isFilling = false;
        }
    }

    public void OnPlayerJumpHandler (object publishedObject)
    {
        OnPlayerJump args = publishedObject as OnPlayerJump;

        interval = 1f / args.Cooldown;
        image.fillAmount = 0f;
        isFilling = true;
        //StartCoroutine(AnimateCircleCoroutine(args.Cooldown));
    }

    private IEnumerator AnimateCircleCoroutine (float interval)
    {
        float step = 0.02f;
        float fillStep = 1f / (interval / step);
        image.fillAmount = 0;
        while (interval > 0)
        {
            image.fillAmount += fillStep;

            interval -= step;
            yield return new WaitForSeconds(step);
        }
    }
}
