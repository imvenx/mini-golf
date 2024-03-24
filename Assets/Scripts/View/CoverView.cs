using System;
using ArcanepadSDK.Models;
using UnityEngine;
public class CoverView : MonoBehaviour
{
    private Action<ArcaneBaseEvent> onCoverStartButtonPress;

    void Start()
    {
        onCoverStartButtonPress = new Action<ArcaneBaseEvent>(OnCoverStartButtonPress);
        Arcane.Msg.On("CoverStartButtonPress", onCoverStartButtonPress);
    }
    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    void OnCoverStartButtonPress(ArcaneBaseEvent e)
    {
        Debug.Log("Cover Start Called !! ");
    }
}

