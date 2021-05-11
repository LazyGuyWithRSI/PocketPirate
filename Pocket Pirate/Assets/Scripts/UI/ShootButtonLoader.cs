using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootButtonLoader : MonoBehaviour
{
    public GameObject PortShootButton;
    public GameObject StarboardShootButton;
    public FloatReference ShootModeFloatRef;

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

    private void LoadControl() // interface maybe?
    {
        if ((ShootControlMode)ShootModeFloatRef.Value == ShootControlMode.BUTTONS)
        {
            PortShootButton.SetActive(true);
            StarboardShootButton.SetActive(true);
        }
        else
        {
            PortShootButton.SetActive(false);
            StarboardShootButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
