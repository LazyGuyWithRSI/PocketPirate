﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireButtonReferenceUpdater : MonoBehaviour
{

    void Start ()
    {
        PubSub.RegisterListener<OnButtonPressedEvent>(OnButtonPressed);
        PubSub.RegisterListener<OnButtonReleasedEvent>(OnButtonReleased);
    }

    public void OnButtonPressed(object publishedEvent)
    {
        OnButtonPressedEvent args = publishedEvent as OnButtonPressedEvent;
    }

    public void OnButtonReleased(object publishedEvent)
    {
        OnButtonReleasedEvent args = publishedEvent as OnButtonReleasedEvent;
    }
}
