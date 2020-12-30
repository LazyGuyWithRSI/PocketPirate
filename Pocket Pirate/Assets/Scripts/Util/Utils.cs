using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float HeadingFromRotation (float yRotation)
    {
        float currentHeading = (yRotation % 360);
        if (currentHeading < 0)
            currentHeading = 360 - currentHeading;
        return currentHeading;
    }

    public static float HeadingFromVector(Vector2 vec)
    {
        return 0;
    }

    public static Vector2 Vector2FromHeading(float heading)
    {
        return new Vector2(Mathf.Cos(heading), Mathf.Sin(heading));
    }

    public static Vector3 Vector3FromHeading (float heading)
    {
        heading = (-heading + 90) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(heading), 0, Mathf.Sin(heading));
    }

    public static Vector3 RandomPointInCircle(float scale)
    {
        return new Vector3(Mathf.Cos(Random.Range(0, 360)) * scale, Mathf.Sin(Random.Range(0, 360)) * scale);
    }
}
