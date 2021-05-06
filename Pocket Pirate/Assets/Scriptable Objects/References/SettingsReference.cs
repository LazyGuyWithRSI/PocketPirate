using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reference/Settings")]
public class SettingsReference : ScriptableObject
{
    public bool Use2AxisJoystick;
    public bool SoundEnabled;
    public float MasterVolume;
    public float MusicVolume;
    public float EffectsVolume;
}
