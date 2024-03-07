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
            ViewManager.selectCoursePanel.RefreshConectedPlayersText();
        });
        pad.On("Wait", (ArcaneBaseEvent e) =>
        {
            isReady = false;
            ViewManager.selectCoursePanel.RefreshConectedPlayersText();
        });

        pad.StartGetQuaternion();
        pad.OnGetQuaternion(new Action<GetQuaternionEvent>(RotatePlayer));
    }

    void RotatePlayer(GetQuaternionEvent e)
    {
        transform.rotation = new Quaternion(e.x, e.y, e.z, e.w);
    }
}
