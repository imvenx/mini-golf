using System;
using System.Reflection;
using ArcanepadSDK.Models;
using ArcanepadSDK.Types;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PadManager : MonoBehaviour
{
    public Button StartGameMenu;
    public Button CalibrateQuaternionButton;
    public TextMeshProUGUI LogsText;

    async void Awake()
    {
        Arcane.Init(new ArcaneInitParams(deviceType: ArcaneDeviceType.pad, padOrientation: AOrientation.Portrait));

        await Arcane.ArcaneClientInitialized();

        // ATTACK
        StartGameMenu.onClick.AddListener(OnStartGameMenuButtonPress);

        // LISTEN FOR AN EVENT SENT TO THIS PAD
        // Arcane.Msg.On("TakeDamage", new Action<TakeDamageEvent>(TakeDamage));

        // CALIBRATE ROTATION
        CalibrateQuaternionButton.onClick.AddListener(() => Arcane.Pad.CalibrateQuaternion());
    }

    void OnStartGameMenuButtonPress()
    {
        Arcane.Msg.EmitToViews(new ArcaneBaseEvent("SelectCourse"));
    }
}
