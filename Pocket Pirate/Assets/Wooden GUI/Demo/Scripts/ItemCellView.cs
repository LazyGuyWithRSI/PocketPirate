using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WoodenGUI
{
	public class ItemCellView : MonoBehaviour
	{
		public string ItemDescription;
		public string ItemId;
		public string ItemIconName;

		public TMP_Text DescriptionText;
		public Image ItemIconImage;
		public Button ExpandButton;
		public Button BuyButton;
		public Transform ExtendedPanel;

		private bool _isExpanded;

		// Start is called before the first frame update
		void Start()
		{
			ExpandButton.onClick.AddListener(OnExpandButtonClick);
			BuyButton.onClick.AddListener(OnBuyButtonClick);
			DescriptionText.text = ItemDescription;
			SetExpanded(_isExpanded);
		}

		private void OnBuyButtonClick()
		{
			Debug.Log("Buy Item " + ItemId);
			UIManager.Instance.Open(Menu.Menus.Confirmation);
		}

		private void OnExpandButtonClick()
		{
			_isExpanded = !_isExpanded;
			SetExpanded(_isExpanded);
		}

		private void SetExpanded(bool value)
		{
			ExtendedPanel.gameObject.SetActive(value);
			ExpandButton.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, value ? -90 : 0);
		}
	}
}