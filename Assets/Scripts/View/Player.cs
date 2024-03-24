using System;
using ArcanepadSDK;
using ArcanepadSDK.Models;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ArcanePad pad { get; private set; }
    public bool isReady { get; private set; } = false;
    public void Initialize(ArcanePad pad)
    {
        this.pad = pad;

        pad.On("Ready", (ArcaneBaseEvent e) =>
        {
            isReady = true;
            ViewManager.selectLevelView.RefreshConectedPlayersText();
        });

        pad.On("Wait", (ArcaneBaseEvent e) =>
        {
            isReady = false;
            ViewManager.selectLevelView.RefreshConectedPlayersText();
        });

        pad.On("QuitLevel", (ArcaneBaseEvent e) =>
        {
            isReady = false;
            ViewManager.selectLevelView.RefreshConectedPlayersText();
            // ViewManager.RefreshUI(UIState.GameCover);
        });

        pad.StartGetQuaternion();
        pad.OnGetQuaternion(new Action<GetQuaternionEvent>(RotatePlayer));
    }

    void RotatePlayer(GetQuaternionEvent e)
    {
        transform.rotation = new Quaternion(e.x, e.y, e.z, e.w);
    }
}
