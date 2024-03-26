using System;
using System.Collections;
using System.Collections.Generic;
using ArcanepadSDK.Models;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelPad : MonoBehaviour
{
    public Button ReadyButton;
    public Button WaitButton;
    public Button BackToCoverButton;

    void Awake()
    {
        ReadyButton = transform.Find("ReadyButton").GetComponent<Button>();
        WaitButton = transform.Find("WaitButton").GetComponent<Button>();
        BackToCoverButton = transform.Find("BackToCoverButton").GetComponent<Button>();
    }

    void Start()
    {
        ReadyButton.onClick.AddListener(() => SetPlayerStateReady());
        WaitButton.onClick.AddListener(() => SetPlayerStateWait());

        BackToCoverButton.onClick.AddListener(() => Arcane.Msg.EmitToViews(new ArcaneBaseEvent("BackToCover")));
    }

    void OnEnable()
    {
        SetPlayerStateWait();
    }

    void SetPlayerStateReady()
    {
        Arcane.Msg.EmitToViews(new ArcaneBaseEvent("PlayerReady"));
        ReadyButton.gameObject.SetActive(false);
        WaitButton.gameObject.SetActive(true);
    }

    void SetPlayerStateWait()
    {
        Arcane.Msg.EmitToViews(new ArcaneBaseEvent("PlayerWait"));
        WaitButton.gameObject.SetActive(false);
        ReadyButton.gameObject.SetActive(true);
    }
}
