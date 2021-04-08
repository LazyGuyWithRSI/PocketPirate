using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatInput : MonoBehaviour
{
    public float JumpAccelRequired = 0.5f;

    public Vector2Reference JoyStickInput;
    public SettingsReference settings;
    public IBoatMover mover;
    public IJump jumper;

    private bool stickReleasedThisFrame = false;

    private bool gameIsOver = false;

    private const string BtnSailsName = "BtnSails";

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<IBoatMover>();
        jumper = GetComponent<IJump>();

        PubSub.RegisterListener<OnGameOver>(OnGameOverHandler);
        PubSub.RegisterListener<OnButtonPressedEvent>(OnButtonPressedHandler);
        PubSub.RegisterListener<OnButtonReleasedEvent>(OnButtonReleasedHandler);

        // TODO sub to jump event or something
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
        {
            mover.SetTurnDirection(0);
            return;
        }

        if (Input.GetKeyDown(KeyCode.V) || Input.acceleration.z > JumpAccelRequired) // DEBUG
        {
            Debug.Log("Request Jump");
            jumper.DoJump();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            PubSub.Publish(new OnPowerupEvent { Type = "Ethereal", Activating = true });
        }

        if (JoyStickInput.Value.x == 0 && JoyStickInput.Value.y == 0)
        {
            
            //mover.SetHeadingToCurrent();
            
            if (Input.GetKey(KeyCode.A))
                mover.SetTurnDirection(-1);
            else if (Input.GetKey(KeyCode.D))
                mover.SetTurnDirection(1);
            else
                mover.SetTurnDirection(0);

            return;
        }

        if (settings.Use2AxisJoystick)
        {
            float angle = Mathf.Atan2(JoyStickInput.Value.x, JoyStickInput.Value.y) * 180 / Mathf.PI;
            angle += 180;

            mover.SetHeading(angle);
        }
        else
        {
            mover.SetTurnDirection(JoyStickInput.Value.x);
        }
    }

    public void OnButtonPressedHandler (object publishedObject)
    {
        OnButtonPressedEvent args = publishedObject as OnButtonPressedEvent;
        if (args.Name.Equals(BtnSailsName) && !gameIsOver)
        {
            mover.SetMoving(0);
        }
    }

    public void OnButtonReleasedHandler (object publishedObject)
    {
        OnButtonReleasedEvent args = publishedObject as OnButtonReleasedEvent;
        if (args.Name.Equals(BtnSailsName) && !gameIsOver)
        {
            mover.SetMoving(1);
        }
    }

    public void OnGameOverHandler (object publishedEvent)
    {
        gameIsOver = true;
    }
}
