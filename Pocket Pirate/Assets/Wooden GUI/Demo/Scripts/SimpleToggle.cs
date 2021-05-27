using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WoodenGUI
{
    public class SimpleToggle : MonoBehaviour
    {
        public TMP_Text Text;
        public string IsOnValue = "Connected";
        public string IsOffValue = "Disconnected";

        private Toggle _toggle;

        // Start is called before the first frame update
        void Start()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            Text.text = isOn ? IsOnValue : IsOffValue;
        }

    }
}