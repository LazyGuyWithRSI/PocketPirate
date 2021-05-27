using UnityEngine.UI;

namespace WoodenGUI
{
    public class MainMenu : Menu
    {
        public Button PlayButton;
        public Button MultiplayButton;
        public Button ShopButton;
        public Button LeadersButton;
        public Button SettingsButton;
        public Button GPGSButton;
        public Button PrefabButton;

        private void Start()
        {
            PlayButton.onClick.AddListener(OnPlayButton);
            MultiplayButton.onClick.AddListener(OnMultiplayButton);
            ShopButton.onClick.AddListener(OnShopButton);
            LeadersButton.onClick.AddListener(OnLeadersButton);
            SettingsButton.onClick.AddListener(OnSettingsButton);
            GPGSButton.onClick.AddListener(OnGPGSButton);
            PrefabButton.onClick.AddListener(OnPrefabButton);
        }

        private void OnGPGSButton()
        {
            UIManager.Instance.Open(Menus.Inventory);
        }

        private void OnSettingsButton()
        {
            UIManager.Instance.Open(Menus.Settings);
        }

        private void OnPlayButton()
        {
            UIManager.Instance.Open(Menus.Complete);
        }

        private void OnMultiplayButton()
        {
            UIManager.Instance.Open(Menus.Level);
        }

        private void OnShopButton()
        {
            UIManager.Instance.OpenWithDelay(Menus.Shop);
        }

        private void OnLeadersButton()
        {
            UIManager.Instance.Open(Menus.Leaders);
        }

        private void OnPrefabButton()
        {
            UIManager.Instance.Open(Menus.Prefabs);
        }
    }
}