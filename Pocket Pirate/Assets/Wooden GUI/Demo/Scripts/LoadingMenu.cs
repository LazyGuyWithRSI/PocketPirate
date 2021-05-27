using System.Collections;
using TMPro;
using UnityEngine;

namespace WoodenGUI
{
    public class LoadingMenu : Menu
    {
        public TMP_Text PercentageText;

        private void Start()
        {
            StartCoroutine(UpdateTime());
        }

        private IEnumerator UpdateTime()
        {
            float elapsedTime = 0;
            while (elapsedTime < UIManager.DELAY)
            {
                elapsedTime += Time.deltaTime;
                float val = elapsedTime / UIManager.DELAY * 100f;
                PercentageText.text = $"{val:F1}%";
                yield return null;
            }
        }
    }
}