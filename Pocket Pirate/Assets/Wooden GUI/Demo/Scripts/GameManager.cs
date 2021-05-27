using UnityEngine;

namespace WoodenGUI
{
    public class GameManager : MonoBehaviour
    {
        private UIManager _uiManager;

        private void Start()
        {
            UIManager.Instance.Open(Menu.Menus.Main);
        }
    }
}