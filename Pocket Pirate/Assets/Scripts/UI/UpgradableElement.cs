using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradableElement : MonoBehaviour
{
    public UpgradablePropertyReference UpgradableProperty;
    public FloatReference Score;

    public TMP_Text TxtName;
    public TMP_Text TxtCurrentValue;
    public TMP_Text TxtNextValue;
    public Button BtnBuy;

    private TMP_Text BtnText;
    private float nextValue;

    // Start is called before the first frame update
    void Start()
    {
        BtnBuy.onClick.AddListener(OnBuyButtonClick);
        Reset();
    }

    private void Reset()
    {
        // populate menu and all that
        TxtName.text = UpgradableProperty.Name;
        TxtCurrentValue.text = UpgradableProperty.Value.ToString();

        nextValue = UpgradableProperty.Value + UpgradableProperty.stepSize;
        BtnText = BtnBuy.GetComponentInChildren<TMP_Text>();
        if (UpgradableProperty.CanIncrement())
        {
            TxtNextValue.text = nextValue.ToString();
            BtnText.text = UpgradableProperty.Cost + "g";
            BtnBuy.interactable = true;
        }
        else
        {
            TxtNextValue.text = "-";
            BtnText.text = "MAX";
            BtnBuy.interactable = false;
        }

        if (Score.Value < UpgradableProperty.Cost)
            BtnBuy.interactable = false;

    }

    private void OnBuyButtonClick()
    {
        Score.Value -= UpgradableProperty.Cost;
        UpgradableProperty.Increment();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
