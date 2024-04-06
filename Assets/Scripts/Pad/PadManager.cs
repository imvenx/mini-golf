using System;
using System.Collections;
using ArcanepadSDK.Models;
using ArcanepadSDK.Types;
using UnityEngine;
using UnityEngine.UI;

public class PadManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject Pads;
    public GameObject CoverPad;
    public GameObject SelectLevelPad;
    public GameObject GamePad;

    public Button ReadyButton;
    public Button WaitButton;
    public Button BackToCoverButton;
    public Button QuitGameButton;
    private GameObject currentActivePad;
    private GameObject transitionAnimPanel;
    async void Start()
    {
        DontDestroyOnLoad(this);

        canvas = GameObject.Find("Canvas");

        Pads = canvas.transform.Find("Pads").gameObject;
        CoverPad = Pads.transform.Find("CoverPad").gameObject;
        SelectLevelPad = Pads.transform.Find("SelectLevelPad").gameObject;
        GamePad = Pads.transform.Find("GamePad").gameObject;
        transitionAnimPanel = canvas.transform.Find("TransitionAnimPanel").gameObject;

        CoverPad.gameObject.SetActive(true);
        currentActivePad = CoverPad;

        SelectLevelPad.SetActive(false);
        GamePad.SetActive(false);

        // BackToCoverButton = SelectCoursePad.transform.Find("BackToCover").GetComponent<Button>();
        // ReadyButton = SelectCoursePad.transform.Find("Ready").GetComponent<Button>();
        // WaitButton = SelectCoursePad.transform.Find("Wait").GetComponent<Button>();
        // QuitGameButton = GamePad.transform.Find("Quit").GetComponent<Button>();

        // RefreshPadUI(Global.gameState.uiState);

        Arcane.Init(new ArcaneInitParams(deviceType: ArcaneDeviceType.pad, padOrientation: AOrientation.Portrait));

        await Arcane.ArcaneClientInitialized();

        Arcane.Msg.On("RefreshUIState", (RefreshUIStateEvent e) => RefreshPadUI(e.uiState));

        // BackToCoverButton.onClick.AddListener(() => Arcane.Msg.EmitToViews(new RefreshUIStateEvent(UIState.GameCover)));

        // ReadyButton.onClick.AddListener(() =>
        // {
        //     Arcane.Msg.EmitToViews(new ArcaneBaseEvent("Ready"));
        //     ReadyButton.gameObject.SetActive(false);
        //     WaitButton.gameObject.SetActive(true);
        // });

        // WaitButton.onClick.AddListener(() =>
        // {
        //     Arcane.Msg.EmitToViews(new ArcaneBaseEvent("Wait"));
        //     ReadyButton.gameObject.SetActive(true);
        //     WaitButton.gameObject.SetActive(false);
        // });

        // QuitGameButton.onClick.AddListener(() => Arcane.Msg.EmitToViews(new RefreshUIStateEvent(UIState.SelectCourse)));

        // Arcane.Msg.On(GameEvent.RefreshUIState, (RefreshUIStateEvent e) =>
        // {
        //     Arcane.Msg.EmitToViews(new ArcaneBaseEvent("QuitLevel"));
        // });

    }

    // void OnRefreshGameState(RefreshGameStateEvent e)
    // {
    //     Global.gameState = e.gameState;

    //     RefreshPadUI(Global.gameState.uiState);
    // }



    void RefreshPadUI(UIState uiState)
    {
        Debug.Log("Refreshed Pad UI State:" + uiState.ToString());

        switch (uiState)
        {
            case UIState.CoverView:

                SwitchActivePanel(true, CoverPad, currentActivePad);
                break;

            case UIState.SelectLevelView:

                SwitchActivePanel(true, SelectLevelPad, currentActivePad);
                break;

            case UIState.InGame:

                SwitchActivePanel(true, GamePad, currentActivePad);

                break;

            // case UIState.Loading:
            //     break;

            case UIState.PlayerDisconnected:
                break;
        }
    }

    public void SwitchActivePanel(bool playTransitionAnim, GameObject panelToShow, GameObject panelToHide)
    {
        if (playTransitionAnim)
        {
            StartCoroutine(PlayTransitionAndSwitch(panelToShow, panelToHide));
        }
        else
        {
            panelToHide.SetActive(false);
            panelToShow.SetActive(true);
            currentActivePad = panelToShow;
        }
    }

    private IEnumerator PlayTransitionAndSwitch(GameObject panelToShow, GameObject panelToHide)
    {
        transitionAnimPanel.SetActive(true);
        Animation anim = transitionAnimPanel.GetComponent<Animation>();

        anim.Play();

        yield return new WaitForSeconds(anim.clip.length / 2);

        panelToHide.SetActive(false);
        panelToShow.SetActive(true);
        currentActivePad = panelToShow;

        yield return new WaitForSeconds(anim.clip.length / 2);

        transitionAnimPanel.SetActive(false);
    }
}
