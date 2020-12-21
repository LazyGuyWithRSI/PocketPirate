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

public class OnShootEvent
{
    public Vector3 Position { get; set; }
    // direction?
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