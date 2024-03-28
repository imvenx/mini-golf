using System;
using System.Collections;
using System.Collections.Generic;
using ArcanepadSDK.Models;
using UnityEngine;

public class GameView : MonoBehaviour
{
    private Action<ArcaneBaseEvent> onQuitGameButtonPress;
    public delegate void QuitGameButtonPresssHandler();
    public event QuitGameButtonPresssHandler QuitGameButtonPress;

    void Start()
    {
        onQuitGameButtonPress = new Action<ArcaneBaseEvent>(OnQuitGameButtonPress);
        Arcane.Msg.On("QuitGameButtonPress", onQuitGameButtonPress);
    }

    void OnQuitGameButtonPress(ArcaneBaseEvent e)
    {
        QuitGameButtonPress.Invoke();
    }
}
