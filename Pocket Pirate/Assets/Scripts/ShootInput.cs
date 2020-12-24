using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootInput : MonoBehaviour
{

    public float TimeScaleWhenAiming = 0.5f;
    public MeshRenderer starboardAimHelper; // TODO these 3 are not about input, should probs move it
    public MeshRenderer portAimHelper;

    public GameObject starboardShootMount;
    private IShooter starboardShooter;

    public GameObject portShootMount;
    private IShooter portShooter;

    // Start is called before the first frame update
    void Start()
    {
        starboardShooter = starboardShootMount.GetComponentInChildren<IShooter>();
        portShooter = portShootMount.GetComponentInChildren<IShooter>();
        starboardShooter.SetName("Starboard");
        portShooter.SetName("Port");


        starboardAimHelper.enabled = false;
        portAimHelper.enabled = false;

        //Debug.Log("unsubbing? : " + PubSub.UnregisterListener<OnButtonReleasedEvent>(OnButtonReleased));
        //PubSub.UnregisterListener<OnButtonPressedEvent>(OnButtonPressed);

        PubSub.RegisterListener<OnButtonReleasedEvent>(OnButtonReleased);
        PubSub.RegisterListener<OnButtonPressedEvent>(OnButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
        // debug controls
        if (Input.GetKeyDown(KeyCode.E))
            starboardShooter.Shoot();

        if (Input.GetKeyDown(KeyCode.Q))
            portShooter.Shoot();
    }

    public void OnButtonPressed (object publishedEvent)
    {
        OnButtonPressedEvent args = publishedEvent as OnButtonPressedEvent;
        if (args.Name == "BtnStarboardFire" && starboardShooter.CanShoot())
        {
            starboardAimHelper.enabled = true;
            Time.timeScale = TimeScaleWhenAiming;
        }
        else if (args.Name == "BtnPortFire" && portShooter.CanShoot())
        {
            portAimHelper.enabled = true;
            Time.timeScale = TimeScaleWhenAiming;
        }
    }

    public void OnButtonReleased (object publishedEvent)
    {
        OnButtonReleasedEvent args = publishedEvent as OnButtonReleasedEvent;
        if (args.Name == "BtnStarboardFire")
        {
            starboardAimHelper.enabled = false;
            Time.timeScale = 1f;
            starboardShooter.Shoot();
        }
        else if (args.Name == "BtnPortFire")
        {
            portAimHelper.enabled = false;
            Time.timeScale = 1f;
            portShooter.Shoot();
        }
    }
}
