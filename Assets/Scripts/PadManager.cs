using System;
using ArcanepadSDK.Models;
using ArcanepadSDK.Types;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PadManager : MonoBehaviour
{
    public Button StartGameMenuButton;
    public Button GoBackToCoverMenuButton;
    public Button CalibrateQuaternionButton;
    public TextMeshProUGUI LogsText;
    public GameObject CoverPad;
    public GameObject SelectCoursePad;

    async void Awake()
    {
        Arcane.Init(new ArcaneInitParams(deviceType: ArcaneDeviceType.pad, padOrientation: AOrientation.Portrait));

        await Arcane.ArcaneClientInitialized();

        StartGameMenuButton.onClick.AddListener(() => Arcane.Msg.EmitToViews(new RefreshUIStateEvent(UIState.SelectCourse)));
        GoBackToCoverMenuButton.onClick.AddListener(() => Arcane.Msg.EmitToViews(new RefreshUIStateEvent(UIState.GameCover)));
        // CalibrateQuaternionButton.onClick.AddListener(() => Arcane.Pad.CalibrateQuaternion());

        // Arcane.Msg.On("RefreshGameState", new Action<RefreshGameStateEvent>(OnRefreshGameState));

        Arcane.Msg.On(GameEvent.RefreshUIState, (RefreshUIStateEvent e) =>
        {
            Global.gameState.uiState = e.uiState;
            Arcane.Msg.EmitToPads(new RefreshUIStateEvent(Global.gameState.uiState));

            RefreshPadUI(Global.gameState.uiState);
        });

    }

    // void OnRefreshGameState(RefreshGameStateEvent e)
    // {
    //     Global.gameState = e.gameState;

    //     RefreshPadUI(Global.gameState.uiState);
    // }


    void RefreshPadUI(UIState uiState)
    {
        switch (uiState)
        {
            case UIState.GameCover:
                CoverPad.SetActive(true);
                SelectCoursePad.SetActive(false);
                break;

            case UIState.SelectCourse:
                CoverPad.SetActive(false);
                SelectCoursePad.SetActive(true);

                break;

            case UIState.Loading:
                break;

            case UIState.PlayerDisconnected:
                break;
        }
    }
}
