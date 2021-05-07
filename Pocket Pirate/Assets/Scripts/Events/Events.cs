using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events // TODO maybe break these out?
{
}

public class OnDeathEvent
{
    public Vector3 Position { get; set; }
    public int Team { get; set; }
}

public class OnReset
{

}

public class OnShootEvent
{
    public Vector3 Position { get; set; }
    // direction?
}

public class OnHitEvent
{
    public Vector3 Position { get; set; }
    public int Team { get; set; }
    public int HitType { get; set; } // TODO enum
}

public class OnButtonPressedEvent
{
    public string Name { get; set; }
}

public class OnButtonReleasedEvent
{
    public string Name { get; set; }
}

public class OnHitWater
{
    public Vector2 Position { get; set; }
}

public class OnGameOver
{
    public bool Died { get; set; }
}

public class OnWaveOver
{
    public int Wave { get; set; }
    public bool Imediate { get; set; }
}

public class OnPlayerFired
{
    public string WeaponName { get; set; }
    public float ReloadDuration { get; set; }
}

public class OnPlayerHealed
{

}

public class OnPlayerReloaded
{
    public string WeaponName { get; set; }
}

public class OnRequestSceneChange
{
    public int SceneIndex { get; set; }
}

public class OnPauseEvent
{
    public bool Paused { get; set; }
}

public class OnShowPausePanel
{
    public bool Show { get; set; }
}

public class OnPlayerJump
{
    public float Cooldown { get; set; }
}

public class ShowUIPanelEvent
{
    public string PanelName { get; set; }
}

public class UIPanelShownEvent
{
    public UIPanel panel { get; set; }
}

public class DamagingExplosionEvent
{
    public Vector3 Position { get; set; }
    public float Radius { get; set; }
    public float Damage { get; set; }
}

public class OnNoUIPanelShowingEvent
{

}

public class OnCoinPickUpEvent
{
    public int Worth { get; set; }
    public Vector3 Position { get; set; }
}

public class OnSpewCoinsEvent
{
    public int Amount { get; set; }
    public Vector3 Position { get; set; }
}

public class OnPowerupEvent
{
    public string Type { get; set; }
    public bool Activating { get; set; }
}