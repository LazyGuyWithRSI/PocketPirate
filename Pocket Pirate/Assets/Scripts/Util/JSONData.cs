using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JSONData
{
    // float references
    public Dictionary<string, FloatReferenceData> FloatReferences;

    // upgradable property references
    public Dictionary<string, UpgradablePropertyReferenceData> UpgradablePropertyReferences;



    // wave definitions
    // <string waveName, enemies <string enemyName, float weight>>

    // special waves
    // <int wave number, <wave names>>

    public override string ToString()
    {
        string str = "";
        str += "Float references: \n";
        foreach (string fr in FloatReferences.Keys)
            str += fr + ", ";

        return str;
    }
}
