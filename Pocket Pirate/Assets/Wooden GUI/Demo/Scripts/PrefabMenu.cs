using UnityEngine.UI;

namespace WoodenGUI
{
    public class PrefabMenu : Menu
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