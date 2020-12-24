using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireButton : MonoBehaviour
{
    public string WeaponName;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        PubSub.RegisterListener<OnPlayerReloaded>(OnPlayerReload);
        PubSub.RegisterListener<OnPlayerFired>(OnPlayerFired);
    }
    public void OnPlayerFired (object publishedEvent)
    {
        OnPlayerFired args = publishedEvent as OnPlayerFired;
        if (args.WeaponName.Equals(WeaponName))
            button.interactable = false; // TODO some cool animations
    }

    public void OnPlayerReload (object publishedEvent)
    {
        OnPlayerReloaded args = publishedEvent as OnPlayerReloaded;
        if (args.WeaponName.Equals(WeaponName))
        {
            Debug.Log("resetting fire button for " + WeaponName);
            button.interactable = true; // TODO some cool animations
        }
    }
}
