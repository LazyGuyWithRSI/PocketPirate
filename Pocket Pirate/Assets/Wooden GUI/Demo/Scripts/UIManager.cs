using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodenGUI
{
    public class UIManager : MonoBehaviour
    {
        public Canvas Canvas;
        public static UIManager Instance;

        private Menu _currentMenu;
        private Stack<Menu> _stack = new Stack<Menu>();

        public const float DELAY = 3.0f;

        void Awake()
        {
            Instance = this;
        }

        public void Open(Menu.Menus menu)
        {
            if(Menu.MenuPaths.TryGetValue(menu, out string path))
            {
                var prefab = Resources.Load<Menu>(path);
                _currentMenu = GameObject.Instantiate<Menu>(prefab, Canvas.transform);
                _stack.Push(_currentMenu);
            }
        }

        public void OpenWithDelay(Menu.Menus menu)
        {
            StartCoroutine(Load(menu));
        }

        private IEnumerator Load(Menu.Menus menu)
        {
            Open(Menu.Menus.Loading);
            yield return new WaitForSeconds(DELAY);
            GoBack();
            Open(menu);
        }

        public void GoBack()
        {
           var item = _stack.Pop();
           Destroy(item.gameObject);
        }
    }
}