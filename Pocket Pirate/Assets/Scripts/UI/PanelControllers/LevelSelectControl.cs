using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectControl : MonoBehaviour, UIPanel
{
    public FloatReference NextWaveReference;
    public FloatReference ScoreReference;

    public GameObject LevelSelectButtonsParent;

    private Animator anim;
    private const string BtnPickLevelName = "BtnPickLevel";

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
        PubSub.RegisterListener<ShowUIPanelEvent>(ShowUIPanelHandler);
        PubSub.RegisterListener<OnButtonReleasedEvent>(OnButtonPressed);

        int counter = 1;

        foreach (Button button in LevelSelectButtonsParent.GetComponentsInChildren<Button>())
        {
            button.gameObject.name = BtnPickLevelName + "-" + counter;
            button.GetComponentInChildren<TMP_Text>().text = "" + counter;
            counter++;
        }

    }
    private void OnButtonPressed(object publishedEvent)
    {
        OnButtonReleasedEvent args = publishedEvent as OnButtonReleasedEvent;
        //Debug.Log("Hey I'm active after all!");
        if (args.Name.Contains(BtnPickLevelName))
        {
            Debug.Log("picking level: " + args.Name.Split('-')[1]);
            NextWaveReference.Value = float.Parse(args.Name.Split('-')[1]);
            ScoreReference.Value = 10000;
            PubSub.Publish<OnRequestSceneChange>(new OnRequestSceneChange { SceneIndex = 2 });
        }
    }

    public void ShowUIPanelHandler(object publishedEvent)
    {
        ShowUIPanelEvent args = publishedEvent as ShowUIPanelEvent;

        if (args.PanelName.Equals(gameObject.name))
        {
            Show();
            PubSub.Publish<UIPanelShownEvent>(new UIPanelShownEvent() { panel = this });
        }
    }

    public void Hide()
    {
        anim.Play("PanelSlideDown");
    }

    public void Show()
    {
        gameObject.SetActive(true);
        anim.Play("PanelSlideUp");
    }
}
