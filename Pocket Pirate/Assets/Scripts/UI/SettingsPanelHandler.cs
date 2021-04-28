using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelHandler : MonoBehaviour
{
    public SettingsReference settings;
    public Toggle Use2AxisJoystickToggle;
    public Toggle SoundToggle;

    void Start ()
    {
        Use2AxisJoystickToggle.isOn = settings.Use2AxisJoystick;
        SoundToggle.isOn = settings.SoundEnabled;
        AudioListener.volume = settings.SoundEnabled ? 1f : 0f; // TODO use audio manager
        Use2AxisJoystickToggle.onValueChanged.AddListener(OnToggle2AxisJoystick);
        SoundToggle.onValueChanged.AddListener(OnToggleSound);
    }

    public void OnToggle2AxisJoystick(bool newVal)
    {
        settings.Use2AxisJoystick = newVal;
    }

    public void OnToggleSound(bool newVal)
    {
        settings.SoundEnabled = newVal;
        AudioListener.volume = settings.SoundEnabled ? 1f : 0f;
    }
}
