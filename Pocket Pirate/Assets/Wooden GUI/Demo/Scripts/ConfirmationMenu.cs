using UnityEngine.UI;

namespace WoodenGUI
{
    public class ConfirmationMenu : Menu
    {
        public Button CancelButton;
        public Button NoButton;
        public Button YesButton;

        // Start is called before the first frame update
        void Start()
        {
            CancelButton.onClick.AddListener(OnNoButton);
            NoButton.onClick.AddListener(OnNoButton);
            YesButton.onClick.AddListener(OnYesButton);
        }

        private void OnNoButton()
        {
            UIManager.Instance.GoBack();
        }
        private void OnYesButton()
        {
            UIManager.Instance.GoBack();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}