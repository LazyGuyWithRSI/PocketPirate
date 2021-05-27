using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WoodenGUI
{
    public class ShopMenu : Menu
    {
        public Button NextButton;
        public Button PreviousButton;

        private int currentPage;

        public List<GameObject> PagePanels;
        public List<Image> PageTabs;
        public Button BackButton;

        private void Start()
        {
            BackButton.onClick.AddListener(OnBackButton);
            NextButton.onClick.AddListener(OnNextPageButton);
            PreviousButton.onClick.AddListener(OnPreviousPageButton);

            LoadPage(currentPage);
        }
        
        private void OnBackButton()
        {
            UIManager.Instance.GoBack();
        }

        private void OnNextPageButton()
        {
            if (currentPage <= PagePanels.Count - 1)
            {
                UnloadPage(currentPage);

                currentPage++;
            
                LoadPage(currentPage);
            }
        }

        private void OnPreviousPageButton()
        {
            if (currentPage > 0)
            {
                UnloadPage(currentPage);

                currentPage--;

                LoadPage(currentPage);
            }
        }

        private void LoadPage(int page)
        {
            if (page >= 0 && page < PagePanels.Count)
            {
                PagePanels[page].SetActive(true);
                PageTabs[page].gameObject.SetActive(true);
            }

            NextButton.gameObject.SetActive(page == 0 || page < PagePanels.Count - 1);
            PreviousButton.gameObject.SetActive(page > 0 || page == PagePanels.Count);
        }

        private void UnloadPage(int page)
        {
            if (page >= 0 && page < PagePanels.Count)
            {
                PagePanels[page].SetActive(false);
                PageTabs[page].gameObject.SetActive(false);
            }
        }
    }
}