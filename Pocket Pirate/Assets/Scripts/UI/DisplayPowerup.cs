using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPowerup : MonoBehaviour
{
    private TMP_Text text;

    private List<string> powerUps;

    // Start is called before the first frame update
    void Start ()
    {
        text = GetComponent<TMP_Text>();
        PubSub.RegisterListener<OnPowerupEvent>(OnPowerupHandler);
        powerUps = new List<string>();
    }

    private void OnPowerupHandler(object pubEvent)
    {
        OnPowerupEvent args = pubEvent as OnPowerupEvent;
        if (args.Activating)
        {
            if (!powerUps.Contains(args.Type))
                powerUps.Add(args.Type);
        }
        else
        {
            if (powerUps.Contains(args.Type))
                powerUps.Remove(args.Type);
        }

        string str = "";
        for (int i = 0; i < powerUps.Count; i++)
        {
            str += powerUps[i];

            if (i != powerUps.Count - 1)
                str += "\n";
        }

        text.text = str;
    }
}
