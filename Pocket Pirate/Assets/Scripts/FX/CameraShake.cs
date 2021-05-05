using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 _originalPos;
    private float _timeAtCurrentFrame;
    private float _timeAtLastFrame;
    private float _fakeDelta;

    private bool gameIsOver = false;

    void Awake ()
    {
        instance = this;
    }

    private void Start ()
    {
        PubSub.RegisterListener<OnGameOver>(OnGameOverHandler);
    }

    public void OnGameOverHandler (object publishedEvent)
    {
        gameIsOver = true;
    }

    void Update ()
    {
        if (gameIsOver)
        {
            Camera.main.orthographicSize = Vector2.Lerp(new Vector2(0, Camera.main.orthographicSize), new Vector2(0, 6f), 0.01f).y;
        }

        // Calculate a fake delta time, so we can Shake while game is paused.
        _timeAtCurrentFrame = Time.realtimeSinceStartup;
        _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
        _timeAtLastFrame = _timeAtCurrentFrame;
    }

    public static void Shake (float duration, float amount)
    {
        instance._originalPos = instance.gameObject.transform.localPosition;
        Debug.Log("Shaking camera");
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.cShake(duration, amount));
    }

    public IEnumerator cShake (float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (duration > 0)
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

            duration -= _fakeDelta;

            yield return null;
        }

        transform.localPosition = _originalPos;
    }
}
