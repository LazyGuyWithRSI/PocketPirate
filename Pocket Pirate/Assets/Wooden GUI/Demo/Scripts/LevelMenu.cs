using UnityEngine.UI;

namespace WoodenGUI
{
    public class LevelMenu : Menu
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