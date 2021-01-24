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

    private bool starboardPressedWhileCanFire = false;
    private bool portPressedWhileCanFire = false;

    private bool inputDisabled = false;
    private bool gameIsOver = false;

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
        if (gameIsOver || inputDisabled)
            return;

        OnButtonPressedEvent args = publishedEvent as OnButtonPressedEvent;
        if (args.Name == "BtnStarboardFire" && starboardShooter.CanShoot())
        {
            starboardAimHelper.enabled = true;
            Time.timeScale = TimeScaleWhenAiming;
            starboardPressedWhileCanFire = true;
        }
        else if (args.Name == "BtnPortFire" && portShooter.CanShoot())
        {
            portAimHelper.enabled = true;
            Time.timeScale = TimeScaleWhenAiming;
            portPressedWhileCanFire = true;
        }
    }

    public void OnButtonReleased (object publishedEvent)
    {
        if (gameIsOver || inputDisabled)
            return;

        OnButtonReleasedEvent args = publishedEvent as OnButtonReleasedEvent;
        if (args.Name == "BtnStarboardFire" && starboardPressedWhileCanFire)
        {
            starboardAimHelper.enabled = false;
            Time.timeScale = 1f;
            starboardPressedWhileCanFire = false;
            starboardShooter.Shoot();
        }
        else if (args.Name == "BtnPortFire" && portPressedWhileCanFire)
        {
            portAimHelper.enabled = false;
            Time.timeScale = 1f;
            portPressedWhileCanFire = false;
            portShooter.Shoot();
        }
    }

    public void OnGameOverHandler (object pubEvent)
    {
        gameIsOver = true;
        StopAllCoroutines();
    }

    public void RapidFire(float duration, float newShootCooldown)
    {
        if (!inputDisabled)
            StartCoroutine(RapidFireCoroutine(duration, newShootCooldown));
    }

    private IEnumerator RapidFireCoroutine(float duration, float newShootCooldown)
    {
        PubSub.Publish(new OnPowerupEvent { Type = "Rapid Fire", Activating = true });
        inputDisabled = true;
        float oldShootCooldownStarboard = starboardShooter.GetCooldown();
        float oldShootCooldownPort = portShooter.GetCooldown();
        starboardShooter.SetCooldown(0);
        portShooter.SetCooldown(0);

        while (duration > 0)
        {
            starboardShooter.Shoot();
            portShooter.Shoot();
            yield return new WaitForSeconds(newShootCooldown);
            duration -= newShootCooldown;
        }

        starboardShooter.SetCooldown(oldShootCooldownStarboard);
        portShooter.SetCooldown(oldShootCooldownPort);
        inputDisabled = false;

        PubSub.Publish(new OnPowerupEvent { Type = "Rapid Fire", Activating = false });
    }
}
