using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArcanepadSDK.Models;
using TMPro;
using UnityEngine;

public class SelectLevelView : MonoBehaviour
{
    private Action<ArcaneBaseEvent> onBackToCoverButtonPress;
    public TextMeshProUGUI connectedPlayersText;

    public delegate void BackToCoverButtonPressHandler();
    public event BackToCoverButtonPressHandler BackToCoverButtonPressed;

    void Awake()
    {
        connectedPlayersText = transform.Find("connectedPlayersText").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        onBackToCoverButtonPress = new Action<ArcaneBaseEvent>(OnBackToCoverButtonPress);
        Arcane.Msg.On("BackToCover", onBackToCoverButtonPress);

        Player.PlayerReadyStateChange += OnPlayerReadyStateChange;
    }

    private void OnPlayerReadyStateChange(Player player, bool isReady)
    {
        RefreshConectedPlayersText();
    }

    void OnEnable()
    {
        // RefreshConectedPlayersText();
    }

    void OnDisable()
    {
    }

    void OnBackToCoverButtonPress(ArcaneBaseEvent e)
    {
        BackToCoverButtonPressed.Invoke();
    }

    private void RefreshConectedPlayersText()
    {
        string connectedPlayers = "Connected Players: ";

        foreach (var player in ViewManager.players)
        {
            connectedPlayers += player.pad.User.name + (player.isReady ? " ready" : " waiting") + "\n";
        }

        connectedPlayersText.text = connectedPlayers;

        if (ViewManager.players.All(player => player.isReady))
        {
            Debug.Log("Starting game...");
            // connectedPlayersText.text = "Starting game...";
            // ViewManager.RefreshUI(UIState.InGame);
        }
    }
}

