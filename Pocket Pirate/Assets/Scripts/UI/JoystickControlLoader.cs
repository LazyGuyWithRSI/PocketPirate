﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControlLoader : MonoBehaviour
{
    public GameObject JoystickSliderPrefab;
    public GameObject Joystick2AxisPrefab;

    public BoolReference Use2AxisJoystickBoolRef;

    private GameObject currentlyLoaded;

    // Start is called before the first frame update
    void Start()
    {
        PubSub.RegisterListener<OnPauseEvent>(OnPauseHandler);

        LoadControl();
    }

    private void OnPauseHandler(object publishedEvent)
    {
        OnPauseEvent args = publishedEvent as OnPauseEvent;
        if (!args.Paused)
        {
            LoadControl();
        }
    }

    private void LoadControl()
    {
        if (currentlyLoaded != null)
        {
            GameObject.Destroy(currentlyLoaded);
            currentlyLoaded = null;
        }

        if (Use2AxisJoystickBoolRef.Value)
            currentlyLoaded = Instantiate(Joystick2AxisPrefab, transform);
        else
            currentlyLoaded = Instantiate(JoystickSliderPrefab, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
