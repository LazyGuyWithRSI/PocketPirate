using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickInputUpdater : MonoBehaviour
{
    public Vector2Reference joyStickInput;
    public Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        joystick = GetComponent<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        joyStickInput.Value.x = joystick.Horizontal;
        joyStickInput.Value.y = joystick.Vertical;
    }
}
