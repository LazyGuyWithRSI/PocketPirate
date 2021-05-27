using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WoodenGUI
{
    public class Selector : MonoBehaviour
    {
        public Button PreviousButton;
        public Button NextButton;
        public TMP_Text Text;
        public List<string> ListItems;

        private int _index;

        void Start()
        {
            PreviousButton.onClick.AddListener(OnPreviousButton);
            NextButton.onClick.AddListener(OnNextButton);
            if(ListItems.Count > 0)
                Text.text = ListItems[0];
        }

        private void OnPreviousButton()
        {
            if (_index > 0)
            {
                _index--;
            }
            else
            {
                _index = 0;
            }
            SetValue();
        }

        private void OnNextButton()
        {
            if (_index < ListItems.Count - 1)
            {
                _index++;
            }
            else
            {
                _index = ListItems.Count - 1;
            }
            SetValue();
        }

        private void SetValue()
        {
            if(ListItems == null)
                return;
            
            if(_index > ListItems.Count - 1 || _index < 0)
                return;
            
            Text.text = ListItems[_index];
        }

        private void OnDestroy()
        {
            PreviousButton.onClick.RemoveAllListeners();
            NextButton.onClick.RemoveAllListeners();
        }
    }
}