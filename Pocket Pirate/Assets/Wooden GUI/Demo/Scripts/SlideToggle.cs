using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WoodenGUI
{
    public class SlideToggle : MonoBehaviour, IPointerDownHandler
    {
        public GameObject OnImage;
        public GameObject OffImage;
        private bool _isOn;
        public Action<bool> OnSlideToggle;
        public void Awake()
        {
            SetToggle(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetToggle(!_isOn);
        }
        
        public void SetToggle(bool isOn)
        {
            if (!_isOn.Equals(isOn))
            {
                _isOn = isOn;
                OnImage.SetActive(isOn);
                OffImage.SetActive(!isOn);
                OnSlideToggle?.Invoke(_isOn);
            }
        }
    }
}