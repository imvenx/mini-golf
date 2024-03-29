using UnityEngine;
using System.Collections.Generic;
using ArcanepadSDK.Models;
using ArcanepadSDK;
using System.Linq;

public class ViewManager : MonoBehaviour
{
    private static ViewManager viewManagerInstance;
    private GameObject canvas;
    private static CoverView coverView;
    private static SelectLevelView selectLevelView;
    private GameObject playerPrefab;
    public static List<Player> players = new List<Player>();
    private LevelLoader levelLoader;
    private GameView gameView;

    async void Awake()
    {
        playerPrefab = Resources.Load<GameObject>("Player");

        canvas = GameObject.Find("Canvas");
        coverView = canvas.transform.Find("coverView").GetComponent<CoverView>();
        selectLevelView = canvas.transform.Find("selectLevelView").GetComponent<SelectLevelView>();
        gameView = GetComponent<GameView>();

        levelLoader = transform.GetComponent<LevelLoader>();

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

        selectLevelView.backToCoverButtonPressed += OnBackToCoverButtonPressed;
        coverView.CoverStartButtonPressed += OnCoverStartButtonPressed;
        selectLevelView.allPlayersReady += OnAllPlayersReady;
        gameView.QuitGameButtonPress += OnQuitGameButtonPress;

    }

    private void OnQuitGameButtonPress()
    {
        RefreshUI(UIState.SelectLevelView);
        levelLoader.UnloadLevel();
    }


    private void OnAllPlayersReady()
    {
        RefreshUI(UIState.InGame);
        levelLoader.LoadLevel("Level_1");
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

    private void RefreshUI(UIState uiState)
    {
        Global.gameState.uiState = uiState;
        Arcane.Msg?.EmitToPads(new RefreshUIStateEvent(Global.gameState.uiState));

        switch (uiState)
        {
            case UIState.CoverView:

                coverView.gameObject.SetActive(true);
                selectLevelView.gameObject.SetActive(false);

                break;

            case UIState.SelectLevelView:

                coverView.gameObject.SetActive(false);
                selectLevelView.gameObject.SetActive(true);

                break;

            case UIState.InGame:

                selectLevelView.gameObject.SetActive(false);
                break;

            // case UIState.Loading:
            //     break;

            case UIState.PlayerDisconnected:
                break;

        }

    }
}

