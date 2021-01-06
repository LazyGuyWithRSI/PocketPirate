using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelControler : MonoBehaviour
{
    public BoolReference IsAPanelShowing;

    public const string TapBackBtnName = "TapBtnBack";
    public const string BackBtnName = "BtnBack";
    public const string BtnSettingsName = "BtnSettings";


    private Stack<UIPanel> panels;

    // Start is called before the first frame update
    void Start()
    {
        IsAPanelShowing.Value = false;
        panels = new Stack<UIPanel>();
        PubSub.RegisterListener<OnButtonReleasedEvent>(OnButtonPressed);
        PubSub.RegisterListener<UIPanelShownEvent>(OnUIPanelShownHandler);
    }

    private void OnButtonPressed (object publishedEvent)
    {
        OnButtonReleasedEvent args = publishedEvent as OnButtonReleasedEvent;
        if (args.Name == TapBackBtnName || args.Name == BackBtnName)
        {
            if (panels.Count != 0)
            {
                // rollback last panel
                panels.Pop().Hide();
            }

            if (panels.Count == 0)
            {
                IsAPanelShowing.Value = false; // TODO even needed anymore with no panels showing event?
                PubSub.Publish(new OnNoUIPanelShowingEvent());
            }
        }

        // TODO move to some UI managment, with SO's and such
        else if (args.Name == BtnSettingsName)
        {
            PubSub.Publish(new ShowUIPanelEvent() { PanelName = "SettingsPanel" });
        }
    }

    private void OnUIPanelShownHandler (object publishedEvent)
    {
        Debug.Log("Firing");
        UIPanelShownEvent args = publishedEvent as UIPanelShownEvent;
        panels.Push(args.panel);
        IsAPanelShowing.Value = true;
    }
}
