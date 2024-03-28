using System.Collections;
using System.Collections.Generic;
using ArcanepadSDK.Models;
using UnityEngine;
using UnityEngine.UI;

public class GamePad : MonoBehaviour
{
    public Button QuitButton;

    void Start()
    {
        QuitButton = transform.Find("QuitButton").GetComponent<Button>();
        QuitButton.onClick.AddListener(() =>
        {
            Arcane.Msg.EmitToViews(new ArcaneBaseEvent("QuitGameButtonPress"));
        });
    }
}
