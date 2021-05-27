using UnityEngine;
using UnityEngine.UI;

namespace WoodenGUI
{
    public class InventoryMenu : Menu
    {
        public ScrollRect ScrollRect;
        public Button GoUpButton;
        public Button GoDownButton;
        public Button GoBackButton;
        
        // Start is called before the first frame update
        void Start()
        {
            ScrollRect.onValueChanged.AddListener(OnScrollPositionChanged);
            GoUpButton.onClick.AddListener(ScrollToTop);
            GoDownButton.onClick.AddListener(ScrollToBottom);
            GoBackButton.onClick.AddListener(OnGoBackButton);
            OnScrollPositionChanged(Vector2.up);
        }
        
        private void OnDestroy()
        {
            ScrollRect.onValueChanged.RemoveAllListeners();
        }
        
        private void OnScrollPositionChanged(Vector2 position)
        {
            GoUpButton.gameObject.SetActive(false);
            GoDownButton.gameObject.SetActive(false);
            if (position.y < 0.1f)
            {
                GoUpButton.gameObject.SetActive(true);
            }
            else if(position.y > 0.9f)
            {
                GoDownButton.gameObject.SetActive(true);
            }
        }

        public void ScrollToTop()
        {
            ScrollRect.normalizedPosition = new Vector2(0, 1);
        }
        public void ScrollToBottom()
        {
            ScrollRect.normalizedPosition = new Vector2(0, 0);
        }
        
        public void OnGoBackButton()
        {
            UIManager.Instance.GoBack();
        }
    }
}