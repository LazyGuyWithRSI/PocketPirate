using UnityEngine.UI;

namespace WoodenGUI
{
    public class CompleteMenu : Menu
    {
        public Button BackButton;
        private void Start()
        {
            BackButton.onClick.AddListener(OnBackButton);
        }

        private void OnBackButton()
        {
            UIManager.Instance.GoBack();
        }
    }
}