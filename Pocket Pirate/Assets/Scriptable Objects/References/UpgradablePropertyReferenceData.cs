using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class UpgradablePropertyReferenceData
{
    public string Name = "N/A";

    public float BaseValue = 1f;
    public float MaxValue = 10f;
    public float StepSize = 1f;

    public int BaseCost = 100;
    public float CostScalingFactor = 1.5f;

    public bool Increasing = true;
    public float RoundFactor = 50;
}
