using UnityEngine;
using System.Collections.Generic;
using ArcanepadSDK.Models;
using System;
using ArcanepadSDK;
using System.Linq;
using UnityEngine.SceneManagement;
public class ViewManager : MonoBehaviour
{
    public static ViewManager viewManagerInstance;
    public GameObject canvas;
    public static CoverView coverView;
    public static SelectLevelView selectLevelView;
    public GameObject playerPrefab;
    public static List<Player> players = new List<Player>();
    public LevelLoader levelLoaer;

    async void Awake()
    {
        playerPrefab = Resources.Load<GameObject>("Player");

        canvas = GameObject.Find("Canvas");
        coverView = canvas.transform.Find("coverView").GetComponent<CoverView>();
        selectLevelView = canvas.transform.Find("selectLevelView").GetComponent<SelectLevelView>();

        levelLoaer = transform.GetComponent<LevelLoader>();

        coverView.gameObject.SetActive(true);

        Arcane.Init();
        var initialState = await Arcane.ArcaneClientInitialized();

        initialState.pads.ForEach(pad => createPlayer(pad));
        Arcane.Msg.On(AEventName.IframePadConnect, (IframePadConnectEvent e) => createPlayerIfDontExist(e));
        Arcane.Msg.On(AEventName.IframePadDisconnect, (IframePadDisconnectEvent e) => destroyPlayer(e));

        // Arcane.Msg.On(GameEvent.RefreshUIState, (RefreshUIStateEvent e) =>
        // {
        //     Global.gameState.uiState = e.uiState;
        //     RefreshUI(Global.gameState.uiState);
        // });

        selectLevelView.BackToCoverButtonPressed += OnBackToCoverButtonPressed;
        coverView.CoverStartButtonPressed += OnCoverStartButtonPressed;

    }

    private void OnCoverStartButtonPressed()
    {
        RefreshUI(UIState.SelectLevelView);
    }


    private void OnBackToCoverButtonPressed()
    {
        RefreshUI(UIState.CoverView);
    }


    void createPlayerIfDontExist(IframePadConnectEvent e)
    {
        var playerExists = players.Any(p => p.pad.IframeId == e.iframeId);
        if (playerExists) return;

        var pad = new ArcanePad(deviceId: e.deviceId, internalId: e.internalId, iframeId: e.iframeId, isConnected: true, user: e.user);
        createPlayer(pad);
    }

    void createPlayer(ArcanePad pad)
    {
        if (string.IsNullOrEmpty(pad.IframeId)) return;

        GameObject newPlayer = Instantiate(playerPrefab);
        newPlayer.transform.parent = transform;

        Player playerComponent = newPlayer.GetComponent<Player>();
        playerComponent.Initialize(pad);

        players.Add(playerComponent);
    }

    void destroyPlayer(IframePadDisconnectEvent e)
    {
        var player = players.FirstOrDefault(p => p.pad.IframeId == e.IframeId);

        if (player == null) Debug.LogError("Player not found to remove on disconnect");

        player.pad.Dispose();
        players.Remove(player);
        Destroy(player.gameObject);
    }

    private static void RefreshUI(UIState uiState)
    {
        Global.gameState.uiState = uiState;
        Arcane.Msg?.EmitToPads(new RefreshUIStateEvent(Global.gameState.uiState));

        switch (uiState)
        {
            case UIState.CoverView:

                if (SceneManager.GetActiveScene().name != "MainMenu") SceneManager.LoadScene("MainMenu");

                coverView.gameObject.SetActive(true);
                selectLevelView.gameObject.SetActive(false);

                break;

            case UIState.SelectLevelView:

                if (SceneManager.GetActiveScene().name != "MainMenu") SceneManager.LoadScene("MainMenu");

                coverView.gameObject.SetActive(false);
                selectLevelView.gameObject.SetActive(true);

                break;

            case UIState.InGame:
                SceneManager.LoadScene("Course1-SkyField");
                break;

            // case UIState.Loading:
            //     break;

            case UIState.PlayerDisconnected:
                break;

        }

    }
}

