using UnityEngine;
using System.Collections.Generic;
using ArcanepadSDK.Models;
using System;
using ArcanepadSDK;
public class ViewManager : MonoBehaviour
{
    public GameObject playerPrefab;
    // public List<Player> players = new List<Player>();
    public GameObject GameCoverView;
    public GameObject SelectCourseView;
    public static List<Player> players = new List<Player>();
    public static bool inGame = false;
    public SelectCourse selectCourseEl;

    async void Awake()
    {
        RefreshUI(Global.gameState.uiState);

        DontDestroyOnLoad(this);

        Arcane.Init();

        var initialState = await Arcane.ArcaneClientInitialized();

        initialState.pads.ForEach(pad =>
        {
            CreatePlayer(pad);
        });

        Arcane.Msg.On(AEventName.IframePadConnect, (IframePadConnectEvent e) =>
        {
            var padExists = players.Find(p => p.pad.DeviceId == e.deviceId) != null;
            if (padExists) return;

            CreatePlayer(new ArcanePad(e.deviceId, e.internalId, e.iframeId, true, e.user));
        });

        Arcane.Msg.On(AEventName.IframePadDisconnect, (IframePadDisconnectEvent e) =>
        {
            var disconectedPad = players.Find(p => p.pad.DeviceId == e.DeviceId);
            if (disconectedPad == null) return;
            players.Remove(disconectedPad);

            selectCourseEl.RefreshConectedPlayersText();
        });

        Arcane.Msg.On(GameEvent.RefreshUIState, (RefreshUIStateEvent e) =>
        {
            Global.gameState.uiState = e.uiState;
            Arcane.Msg.EmitToPads(new RefreshUIStateEvent(Global.gameState.uiState));

            RefreshUI(Global.gameState.uiState);
        });
    }

    void CreatePlayer(ArcanePad pad)
    {
        var newPlayer = new Player(pad);
        players.Add(newPlayer);
        Arcane.Msg.EmitToPads(new RefreshUIStateEvent(Global.gameState.uiState));

        pad.On("Ready", (ArcaneBaseEvent e) =>
        {
            newPlayer.isReady = true;
            selectCourseEl.RefreshConectedPlayersText();
        });
        pad.On("Wait", (ArcaneBaseEvent e) =>
        {
            newPlayer.isReady = false;
            selectCourseEl.RefreshConectedPlayersText();
        });

        selectCourseEl.RefreshConectedPlayersText();
    }

    void RefreshUI(UIState uiState)
    {
        switch (uiState)
        {
            case UIState.GameCover:
                GameCoverView.SetActive(true);
                SelectCourseView.SetActive(false);
                break;

            case UIState.SelectCourse:
                GameCoverView.SetActive(false);
                SelectCourseView.SetActive(true);
                break;

            case UIState.Loading:
                break;

            case UIState.PlayerDisconnected:
                break;
        }
    }

    // void createPlayerIfDontExist(IframePadConnectEvent e)
    // {
    //     var playerExists = players.Any(p => p.Pad.IframeId == e.iframeId);
    //     if (playerExists) return;

    //     var pad = new ArcanePad(deviceId: e.deviceId, internalId: e.internalId, iframeId: e.iframeId, isConnected: true, user: e.user);
    //     createPlayer(pad);
    // }

    // void createPlayer(ArcanePad pad)
    // {
    //     if (string.IsNullOrEmpty(pad.IframeId)) return;

    //     GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    //     Player playerComponent = newPlayer.GetComponent<Player>();
    //     playerComponent.Initialize(pad);

    //     players.Add(playerComponent);
    // }

    // void destroyPlayer(IframePadDisconnectEvent e)
    // {
    //     var player = players.FirstOrDefault(p => p.Pad.IframeId == e.IframeId);

    //     if (player == null) Debug.LogError("Player not found to remove on disconnect");

    //     player.Pad.Dispose();
    //     players.Remove(player);
    //     Destroy(player.gameObject);
    // }

}

