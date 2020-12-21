using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEventFirer : MonoBehaviour
{
    public void OnButtonDown()
    {
        PubSub.Publish<OnButtonPressedEvent>(new OnButtonPressedEvent() { Name = gameObject.name });
    }

    public void OnButtonUp()
    {
        PubSub.Publish<OnButtonReleasedEvent>(new OnButtonReleasedEvent() { Name = gameObject.name });
    }
}
