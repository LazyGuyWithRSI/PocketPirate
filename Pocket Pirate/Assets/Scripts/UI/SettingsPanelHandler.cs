using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelHandler : MonoBehaviour
{
    public SettingsReference settings;
    public Toggle Use2AxisJoystickToggle;

    void Start ()
    {
        Use2AxisJoystickToggle.isOn = settings.Use2AxisJoystick;
        Use2AxisJoystickToggle.onValueChanged.AddListener(OnToggle2AxisJoystick);
    }

    public void OnToggle2AxisJoystick(bool newVal)
    {
        settings.Use2AxisJoystick = newVal;
    }
}
