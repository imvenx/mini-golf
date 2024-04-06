using UnityEngine;
using System.Collections.Generic;
using ArcanepadSDK.Models;
using ArcanepadSDK;
using System.Linq;
using System.Collections;
using System;

public class ViewManager : MonoBehaviour
{
    private GameObject canvas;
    private static CoverView coverView;
    private static SelectLevelView selectLevelView;
    private static GameObject loadingView;
    private GameObject playerPrefab;
    public static List<Player> players = new List<Player>();
    private LevelLoader levelLoader;
    private GameView gameView;
    private GameObject currentActiveView;
    private GameObject transitionAnimPanel;

    async void Awake()
    {
        playerPrefab = Resources.Load<GameObject>("Player");

        canvas = GameObject.Find("Canvas");
        coverView = canvas.transform.Find("CoverView").GetComponent<CoverView>();
        selectLevelView = canvas.transform.Find("SelectLevelView").GetComponent<SelectLevelView>();
        loadingView = canvas.transform.Find("LoadingView").gameObject;
        transitionAnimPanel = canvas.transform.Find("TransitionAnimPanel").gameObject;
        gameView = GetComponent<GameView>();

        levelLoader = transform.GetComponent<LevelLoader>();

        coverView.gameObject.SetActive(true);
        currentActiveView = coverView.gameObject;


        Arcane.Init();
        var initialState = await Arcane.ArcaneClientInitialized();

        initialState.pads.ForEach(pad => createPlayer(pad));
        Arcane.Msg.On(AEventName.IframePadConnect, (IframePadConnectEvent e) => createPlayerIfDontExist(e));
        Arcane.Msg.On(AEventName.IframePadDisconnect, (IframePadDisconnectEvent e) => destroyPlayer(e));

        selectLevelView.backToCoverButtonPressed += OnBackToCoverButtonPressed;
        coverView.CoverStartButtonPressed += OnCoverStartButtonPressed;
        selectLevelView.allPlayersReady += OnAllPlayersReady;
        gameView.QuitGameButtonPress += OnQuitGameButtonPress;
        levelLoader.levelLoaded += OnLevelLoaded;
        levelLoader.levelUnloaded += OnLevelUnloaded;
    }

    private void OnLevelUnloaded()
    {
        RefreshUI(UIState.SelectLevelView);
    }


    private void OnLevelLoaded()
    {
        RefreshUI(UIState.InGame);
    }


    private void OnQuitGameButtonPress()
    {
        RefreshUI(UIState.Loading);
        levelLoader.UnloadLevel();
    }

    private void OnAllPlayersReady()
    {
        RefreshUI(UIState.Loading);
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

                SwitchActivePanel(true, coverView.gameObject, currentActiveView);
                break;

            case UIState.SelectLevelView:
                SwitchActivePanel(true, selectLevelView.gameObject, currentActiveView);
                break;

            case UIState.InGame:

                SwitchActivePanel(false, null, currentActiveView);
                break;

            case UIState.Loading:
                SwitchActivePanel(true, loadingView.gameObject, currentActiveView);
                break;

                // case UIState.PlayerDisconnected:
                //     break;

        }
    }

    // void SwitchActivePanel(bool playTransitionAnim, GameObject panelToShow, GameObject panelToHide)
    // {
    //     if (playTransitionAnim)
    //     {
    //         transitionAnimPanel.SetActive(true);
    //         transitionAnimPanel.GetComponent<Animation>().Play();
    //     }

    //     panelToHide.SetActive(false);
    //     panelToShow.SetActive(true);
    //     currentActiveView = panelToShow;
    // }

    public void SwitchActivePanel(bool playTransitionAnim, GameObject panelToShow, GameObject panelToHide)
    {
        if (playTransitionAnim)
        {
            StartCoroutine(PlayTransitionAndSwitch(panelToShow, panelToHide));
        }
        else
        {
            panelToHide?.SetActive(false);
            panelToShow?.SetActive(true);
            currentActiveView = panelToShow;
        }
    }

    private IEnumerator PlayTransitionAndSwitch(GameObject panelToShow, GameObject panelToHide)
    {
        transitionAnimPanel.SetActive(true);
        Animation anim = transitionAnimPanel.GetComponent<Animation>();

        anim.Play();

        yield return new WaitForSeconds(anim.clip.length / 2);

        panelToHide?.SetActive(false);
        panelToShow?.SetActive(true);
        currentActiveView = panelToShow;

        yield return new WaitForSeconds(anim.clip.length / 2);

        transitionAnimPanel.SetActive(false);
    }
}

