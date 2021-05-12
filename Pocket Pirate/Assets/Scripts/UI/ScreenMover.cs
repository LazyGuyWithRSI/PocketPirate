using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMover : MonoBehaviour
{
    public Button BtnLeft;
    public Button BtnRight;
    public RectTransform Screens;
    public float DistanceToSnap = 900f;

    //private LeanSnap screenSnap;

    // Start is called before the first frame update
    void Start()
    {
        //screenSnap = GetComponentInChildren<LeanSnap>();

        BtnLeft.onClick.AddListener(OnLeftButtonPressed);
        BtnRight.onClick.AddListener(OnRightButtonPressed);
    }

    public void OnLeftButtonPressed()
    {
        Screens.anchoredPosition += new Vector2(DistanceToSnap, 0);
    }

    public void OnRightButtonPressed()
    {
        Screens.anchoredPosition += new Vector2(-DistanceToSnap, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
