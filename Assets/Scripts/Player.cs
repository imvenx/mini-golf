using System;
using ArcanepadExample;
using ArcanepadSDK;
using ArcanepadSDK.Models;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ArcanePad Pad { get; private set; }

    async void Start()
    {
        var initialState = await Arcane.ArcaneClientInitialized();

        Initialize();
    }
    public void Initialize()
    {
        Pad = Arcane.Pads[0];

        Pad.StartGetQuaternion();
        Pad.OnGetQuaternion(new Action<GetQuaternionEvent>(RotatePlayer));

        GameObject pointerObject = GameObject.Find("Pointer");
    }

    void RotatePlayer(GetQuaternionEvent e)
    {
        transform.rotation = new Quaternion(e.x, e.y, e.z, e.w);
    }
}