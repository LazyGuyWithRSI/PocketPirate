using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingController : MonoBehaviour
{
    public string SettingName;
    public GameObject Toggle;
    public GameObject Dropdown;
    public TMP_Text Label;

    public BoolReference BoolReference;
    public FloatReference DropDownFloatReference;

    // Start is called before the first frame update
    void Start()
    {
        Label.text = SettingName;

        if (BoolReference != null)
        {
            Toggle.SetActive(true);
            Toggle.GetComponent<LeanToggle>().On = BoolReference.Value;
        }
        else if (DropDownFloatReference != null)
        {
            Dropdown.SetActive(true);
            Dropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(OnDropdownValueChanged);
            Dropdown.GetComponent<TMP_Dropdown>().value = (int)DropDownFloatReference.Value;
        }
    }

    public void OnDropdownValueChanged(int value)
    {
        DropDownFloatReference.Value = value;
    }

    public void OnToggleOn()
    {
        BoolReference.Value = true;
    }

    public void OnToggleOff()
    {
        BoolReference.Value = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
