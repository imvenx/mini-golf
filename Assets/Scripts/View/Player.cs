using System;
using ArcanepadSDK;
using ArcanepadSDK.Models;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ArcanePad pad { get; private set; }
    public bool isReady { get; private set; } = false;

    public delegate void PlayerReadyStateChangeHandler(Player player, bool isReady);
    public static event PlayerReadyStateChangeHandler PlayerReadyStateChange;

    public void Initialize(ArcanePad pad)
    {
        this.pad = pad;

        pad.On("PlayerReady", (ArcaneBaseEvent e) =>
        {
            isReady = true;
            PlayerReadyStateChange?.Invoke(this, isReady);
        });

        pad.On("PlayerWait", (ArcaneBaseEvent e) =>
        {
            isReady = false;
            PlayerReadyStateChange?.Invoke(this, isReady);
        });

        // pad.On("QuitLevel", (ArcaneBaseEvent e) =>
        // {
        //     isReady = false;
        //     ViewManager.selectLevelView.RefreshConectedPlayersText();
        //     // ViewManager.RefreshUI(UIState.GameCover);
        // });

        pad.StartGetQuaternion();
        pad.OnGetQuaternion(new Action<GetQuaternionEvent>(RotatePlayer));
    }

    void RotatePlayer(GetQuaternionEvent e)
    {
        transform.rotation = new Quaternion(e.x, e.y, e.z, e.w);
    }
}
