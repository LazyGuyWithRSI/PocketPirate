using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSegment : MonoBehaviour
{
    [SerializeField] private Sprite OnSprite;
    [SerializeField] private Sprite OffSprite;

    [HideInInspector]
    public bool isOn = false;

    private Image image;

    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponentInChildren<Image>();
        UpdateSprite();
    }

    public void SetOn(bool newIsOn)
    {
        isOn = newIsOn;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        image.sprite = isOn ? OnSprite : OffSprite;
    }
}
