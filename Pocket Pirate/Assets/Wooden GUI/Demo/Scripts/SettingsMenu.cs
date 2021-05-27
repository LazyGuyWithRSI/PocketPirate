using UnityEngine.UI;

namespace WoodenGUI
{
    public class SettingsMenu : Menu
    {
        public Toggle FacebookToggle;
        public Toggle GameServiceToggle;
        public SlideToggle MusicToggle;
        public SlideToggle SoundToggle;
        public Button BackButton;
        public Button CloseButton;
        public Button AboutButton;
        public Button RestoreButton;

        private void Start()
        {
            FacebookToggle.onValueChanged.AddListener(OnFacebookToggle);    
            GameServiceToggle.onValueChanged.AddListener(OnGameServiceToggle);    
            MusicToggle.OnSlideToggle += OnMusicToggle;  
            SoundToggle.OnSlideToggle += OnSoundToggle;    
            BackButton.onClick.AddListener(OnBackButton);
            CloseButton.onClick.AddListener(OnBackButton);
            AboutButton.onClick.AddListener(OnAboutButton);
            RestoreButton.onClick.AddListener(OnRestoreButton);
        }

        private void OnRestoreButton()
        {
        }

        private void OnAboutButton()
        {
        }

        private void OnBackButton()
        {
            UIManager.Instance.GoBack();
        }

        private void OnFacebookToggle(bool arg0)
        {
            
        }
        private void OnSoundToggle(bool arg0)
        {
            
        }
        private void OnMusicToggle(bool arg0)
        {
            
        }
        private void OnGameServiceToggle(bool arg0)
        {
            
        }

        private void OnDestroy()
        {
            FacebookToggle.onValueChanged.RemoveAllListeners();
            GameServiceToggle.onValueChanged.RemoveAllListeners(); 
            MusicToggle.OnSlideToggle -= OnMusicToggle;  
            SoundToggle.OnSlideToggle -= OnSoundToggle;    
            BackButton.onClick.RemoveAllListeners();
            CloseButton.onClick.RemoveAllListeners();
            RestoreButton.onClick.RemoveAllListeners();
            AboutButton.onClick.RemoveAllListeners();
        }
    }
}