using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleFillerDrift : MonoBehaviour
{
    private Image image;
    private Button button;
    private bool isFilling = false;
    private float interval = 0;

    // Start is called before the first frame update
    void Start ()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        PubSub.RegisterListener<OnPlayerDriftOver>(OnPlayerDriftOverHandler);
    }

    void Update()
    {
        if (isFilling)
        {
            image.fillAmount = Mathf.MoveTowards(image.fillAmount, 1, interval * Time.deltaTime);
            if (image.fillAmount >= 1)
            {
                isFilling = false;
                button.interactable = true;
            }
        }
    }

    public void OnPlayerDriftOverHandler(object publishedObject)
    {
        OnPlayerDriftOver args = publishedObject as OnPlayerDriftOver;

        interval = 1f / args.Cooldown;
        image.fillAmount = 0f;
        isFilling = true;
        button.interactable = false;
        //StartCoroutine(AnimateCircleCoroutine(args.ReloadDuration));
    }

    private IEnumerator AnimateCircleCoroutine (float interval)
    {
        float step = 0.03f;
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
