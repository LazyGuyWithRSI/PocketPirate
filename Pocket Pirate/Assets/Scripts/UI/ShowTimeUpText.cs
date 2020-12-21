using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTimeUpText : MonoBehaviour
{
    public FloatReference GameTimeLeft;

    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameTimeLeft.Value <= 0)
            text.enabled = true;
    }
}
