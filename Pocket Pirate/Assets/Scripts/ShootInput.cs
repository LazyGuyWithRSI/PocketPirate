using System;
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

    public float swipeThreshold = 40f;
    public float timeThreshold = 0.3f;
    public FloatReference PlayerHeading;

    private Vector2 _fingerDown;
    private DateTime _fingerDownTime;
    private Vector2 _fingerUp;
    private DateTime _fingerUpTime;

    // Start is called before the first frame update
    void Start()
    {
        starboardShooter = starboardShootMount.GetComponentInChildren<IShooter>();
        portShooter = portShootMount.GetComponentInChildren<IShooter>();
        starboardShooter.SetName("Starboard");
        portShooter.SetName("Port");


        //starboardAimHelper.enabled = false;
        //portAimHelper.enabled = false;


        //Debug.Log("unsubbing? : " + PubSub.UnregisterListener<OnButtonReleasedEvent>(OnButtonReleased));
        //PubSub.UnregisterListener<OnButtonPressedEvent>(OnButtonPressed);

        PubSub.RegisterListener<OnButtonReleasedEvent>(OnButtonReleased);
        PubSub.RegisterListener<OnButtonPressedEvent>(OnButtonPressed);
        PubSub.RegisterListener<OnGameOver>(OnGameOverHandler);

        PubSub.RegisterListener<OnPlayerFired>(OnPlayerFired);
        PubSub.RegisterListener<OnPlayerReloaded>(OnPlayerReload);
    }

    // Update is called once per frame
    void Update()
    {
        // debug controls
        if (Input.GetKeyDown(KeyCode.E))
            starboardShooter.Shoot();

        if (Input.GetKeyDown(KeyCode.Q))
            portShooter.Shoot();

        if (Input.GetMouseButtonDown(0))
        {
            _fingerDown = Input.mousePosition;
            _fingerUp = Input.mousePosition;
            _fingerDownTime = DateTime.Now;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _fingerDown = Input.mousePosition;
            _fingerUpTime = DateTime.Now;
            CheckSwipe();
        }

        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _fingerDown = touch.position;
                _fingerUp = touch.position;
                _fingerDownTime = DateTime.Now;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                _fingerDown = touch.position;
                _fingerUpTime = DateTime.Now;
                CheckSwipe();
            }
        }
    }

    private void CheckSwipe()
    {
        var duration = (float)_fingerUpTime.Subtract(_fingerDownTime).TotalSeconds;
        var dirVector = _fingerUp - _fingerDown;

        if (duration > timeThreshold) return;
        if (dirVector.magnitude < swipeThreshold) return;

        var direction = Vector2.Angle(Vector2.up, dirVector); //dirVector.Rotation(180f).Round();
        if (_fingerUp.x < _fingerDown.x)
            direction = 360f - direction;

        //print("finger down: " + _fingerDown + ", finger up:" + _fingerUp + ", swipe dir: " + direction + ", player heading: " + PlayerHeading.Value);

        // get side of ship
        float difference = PlayerHeading.Value - direction;
        //print(difference);

        if (_fingerUp.x < _fingerDown.x)
            starboardShooter.Shoot();
        else
            portShooter.Shoot();

        /*
        if (difference > 0f)
        {
            if (difference > 180f)
                starboardShooter.Shoot();
            else
                portShooter.Shoot();
        }
        else
        {
            if (difference < -180f)
                portShooter.Shoot();
            else
                starboardShooter.Shoot();
        }*/

        /*
        if (direction >= 45 && direction < 135) onSwipeUp.Invoke();
        else if (direction >= 135 && direction < 225) onSwipeRight.Invoke();
        else if (direction >= 225 && direction < 315) onSwipeDown.Invoke();
        else if (direction >= 315 && direction < 360 || direction >= 0 && direction < 45) onSwipeLeft.Invoke();
        */
    }

    public void OnPlayerFired(object publishedEvent)
    {
        OnPlayerFired args = publishedEvent as OnPlayerFired;
        if (args.WeaponName.Equals("Port"))
            portAimHelper.enabled = false;
        else if (args.WeaponName.Equals("Starboard"))
            starboardAimHelper.enabled = false;

    }

    public void OnPlayerReload(object publishedEvent)
    {
        OnPlayerReloaded args = publishedEvent as OnPlayerReloaded;
        if (args.WeaponName.Equals("Port"))
            portAimHelper.enabled = true;
        else if (args.WeaponName.Equals("Starboard"))
            starboardAimHelper.enabled = true;
    }

    public void OnButtonPressed (object publishedEvent)
    {
        if (gameIsOver || inputDisabled)
            return;

        OnButtonPressedEvent args = publishedEvent as OnButtonPressedEvent;
        if (args.Name == "BtnStarboardFire" && starboardShooter.CanShoot() && Time.timeScale != 0f)
        {
            //starboardAimHelper.enabled = true;
            portAimHelper.enabled = false;
            Time.timeScale = TimeScaleWhenAiming;
            starboardPressedWhileCanFire = true;
        }
        else if (args.Name == "BtnPortFire" && portShooter.CanShoot() && Time.timeScale != 0f)
        {
            //portAimHelper.enabled = true;
            starboardAimHelper.enabled = false;
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
            //starboardAimHelper.enabled = false;
            portAimHelper.enabled = true;
            Time.timeScale = 1f;
            starboardPressedWhileCanFire = false;
            starboardShooter.Shoot();
        }
        else if (args.Name == "BtnPortFire" && portPressedWhileCanFire)
        {
            //portAimHelper.enabled = false;
            starboardAimHelper.enabled = true;
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
