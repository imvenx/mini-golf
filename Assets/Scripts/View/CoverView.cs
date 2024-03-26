using System;
using ArcanepadSDK.Models;
using UnityEngine;
public class CoverView : MonoBehaviour
{
    private Action<ArcaneBaseEvent> onCoverStartButtonPress;
    public delegate void CoverStartButtonPressedsHandler();
    public event CoverStartButtonPressedsHandler CoverStartButtonPressed;

    void Start()
    {
        onCoverStartButtonPress = new Action<ArcaneBaseEvent>(OnCoverStartButtonPress);
        Arcane.Msg.On("CoverStartButtonPress", onCoverStartButtonPress);
    }

    void OnCoverStartButtonPress(ArcaneBaseEvent e)
    {
        CoverStartButtonPressed.Invoke();
    }
}

