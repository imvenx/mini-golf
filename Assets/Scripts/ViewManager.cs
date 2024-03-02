using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ArcanepadSDK.Models;
using ArcanepadSDK;
using System;
public class ViewManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public List<Player> players = new List<Player>();

    async void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        // INITIALIZE CLIENT
        Arcane.Init();

        // WAIT UNTIL CLIENT IS INITIALIZED
        var initialState = await Arcane.ArcaneClientInitialized();

        // CREATE A PLAYER FOR EACH GAMEPAD THAT WAS CONNECTED BEFORE INITIALIZATION
        // initialState.pads.ForEach(pad => createPlayer(pad));

        // // CREATE A PLAYER FOR EACH GAMEPAD THAT CONNECTS AFTER INITIALIZATION
        // Arcane.Msg.On(AEventName.IframePadConnect, new Action<IframePadConnectEvent>(createPlayerIfDontExist));

        // // DESTROY PLAYER ON GAMEPAD DISCONNECT
        // Arcane.Msg.On(AEventName.IframePadDisconnect, new Action<IframePadDisconnectEvent>(destroyPlayer));

        Arcane.Msg.On("SelectCourse", () =>
        {

        });
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

