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
    public static GameObject gameCoverPanel;
    public static SelectCourse selectCoursePanel;
    public GameObject playerPrefab;
    public static List<Player> players = new List<Player>();

    async void Awake()
    {
        if (viewManagerInstance == null)
        {
            viewManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (viewManagerInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        var arcaneLibraryPrefab = Resources.Load<GameObject>("ArcaneLibrary/Arcane_Library");
        GameObject arcaneLibraryInstance = Instantiate(arcaneLibraryPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);

        playerPrefab = Resources.Load<GameObject>("Player");

        canvas = GameObject.Find("Canvas");
        gameCoverPanel = canvas.transform.Find("gameCoverPanel").gameObject;
        selectCoursePanel = canvas.transform.Find("selectCoursePanel").GetComponent<SelectCourse>();

        RefreshUI(Global.gameState.uiState);

        Arcane.Init();
        var initialState = await Arcane.ArcaneClientInitialized();

        initialState.pads.ForEach(pad =>
        {
            createPlayer(pad);
        });

        Arcane.Msg.On(AEventName.IframePadConnect, (IframePadConnectEvent e) =>
        {
            createPlayerIfDontExist(e);
        });

        Arcane.Msg.On(AEventName.IframePadDisconnect, (IframePadDisconnectEvent e) =>
        {
            destroyPlayer(e);
        });

        Arcane.Msg.On(GameEvent.RefreshUIState, (RefreshUIStateEvent e) =>
        {
            Global.gameState.uiState = e.uiState;
            RefreshUI(Global.gameState.uiState);
        });
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


        selectCoursePanel.RefreshConectedPlayersText();
    }

    void destroyPlayer(IframePadDisconnectEvent e)
    {
        var player = players.FirstOrDefault(p => p.pad.IframeId == e.IframeId);

        if (player == null) Debug.LogError("Player not found to remove on disconnect");

        player.pad.Dispose();
        players.Remove(player);
        Destroy(player.gameObject);

        selectCoursePanel.RefreshConectedPlayersText();
    }

    public static void RefreshUI(UIState uiState)
    {
        Arcane.Msg?.EmitToPads(new RefreshUIStateEvent(Global.gameState.uiState));


        switch (uiState)
        {
            case UIState.GameCover:

                if (SceneManager.GetActiveScene().name != "MainMenu") SceneManager.LoadScene("MainMenu");

                gameCoverPanel.SetActive(true);
                selectCoursePanel.gameObject.SetActive(false);
                break;

            case UIState.SelectCourse:

                if (SceneManager.GetActiveScene().name != "MainMenu") SceneManager.LoadScene("MainMenu");

                gameCoverPanel.SetActive(false);
                selectCoursePanel.gameObject.SetActive(true);
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

