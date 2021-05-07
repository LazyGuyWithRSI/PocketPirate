using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCompleteTextController : MonoBehaviour
{
    public float ShowDuration = 2f;
    public TMP_Text text;
    public FloatReference CurrentWave;

    // Start is called before the first frame update
    void Start()
    {
        PubSub.RegisterListener<OnWaveOver>(OnWaveOverHandler);
        StartCoroutine(ShowTextCoroutine(ShowDuration, ""));
    }

    private void OnWaveOverHandler(object publishedObject)
    {
        StartCoroutine(ShowTextCoroutine(ShowDuration, "  Complete"));
    }

    private IEnumerator ShowTextCoroutine(float duration, string postText)
    {
        text.text = "Wave  " + CurrentWave.Value + postText;
        yield return new WaitForSeconds(duration);
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
