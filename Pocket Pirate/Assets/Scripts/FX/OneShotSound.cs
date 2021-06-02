using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotSound : MonoBehaviour
{
    AudioSource audio;
    public AudioClip[] clip;

    public float PitchLower = 0.9f;
    public float PitchUpper = 1.1f;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlayOneShot()
    {
        audio.pitch = Random.Range(PitchLower, PitchUpper);
        audio.PlayOneShot(clip[Random.Range(0, clip.Length)], audio.volume);
    }
}
