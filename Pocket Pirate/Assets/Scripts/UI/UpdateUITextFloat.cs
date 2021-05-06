using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUITextFloat : MonoBehaviour
{
    public FloatReference FloatReference;
    public bool OnlyAtStart = false;
    public string PreText = "";
    public string PostText = "";

    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();

        text.text = PreText + FloatReference.Value.ToString() + PostText;
    }

    // Update is called once per frame
    void Update()
    {
        if (!OnlyAtStart)
            text.text = PreText + FloatReference.Value.ToString() + PostText;
    }
}
