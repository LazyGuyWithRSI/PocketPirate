using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTimeUpText : MonoBehaviour
{
    public FloatReference GameTimeLeft;

    private TMP_Text text;

    private const string DeathText = "YOU DIED!";
    private const string TimeUpText = "TIME UP!";
    private bool timeUp = false;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = DeathText;
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeUp && GameTimeLeft.Value <= 0)
        {
            text.text = TimeUpText;
            timeUp = false;
        }
    }
}
