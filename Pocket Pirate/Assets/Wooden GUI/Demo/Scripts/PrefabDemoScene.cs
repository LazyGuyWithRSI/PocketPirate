using UnityEngine;
using UnityEngine.UI;
using WoodenGUI;

public class PrefabDemoScene : MonoBehaviour
{

    public Button BackButton;
    
    // Start is called before the first frame update
    void Start()
    {
        BackButton.onClick.AddListener(OnBackButton);    
    }

    private void OnBackButton()
    {
        UIManager.Instance.GoBack();
    }
}
