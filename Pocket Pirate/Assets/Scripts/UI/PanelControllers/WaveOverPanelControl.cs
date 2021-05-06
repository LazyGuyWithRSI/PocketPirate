using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveOverPanelControl : MonoBehaviour
{
    public float AnimDelay = 1f;
    public TMP_Text WaveCompleteText;
    public FloatReference CurrentWave;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //gameObject.SetActive(false);
        PubSub.RegisterListener<OnWaveOver>(OnWaveOverHandle);

        // disable panel on start up, some focus issue means spacebar hits the try again button...
        gameObject.SetActive(false);
    }

    public void OnWaveOverHandle(object publishedEvent)
    {
        OnWaveOver args = publishedEvent as OnWaveOver;
        gameObject.SetActive(true);
        if (args.Imediate)
            StartCoroutine(DelayAnim(0f));
        else
            StartCoroutine(DelayAnim(AnimDelay));
    }

    private IEnumerator DelayAnim(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        WaveCompleteText.text = "Wave " + CurrentWave.Value + " Complete";
        anim.Play("PanelSlideUp");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
