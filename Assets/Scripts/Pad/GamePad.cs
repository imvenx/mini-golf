using ArcanepadSDK.Models;
using UnityEngine;
using UnityEngine.UI;

public class GamePad : MonoBehaviour
{
    public Button QuitButton;

    void Awake()
    {
        QuitButton.onClick.AddListener(() =>
        {
            Arcane.Msg.EmitToViews(new ArcaneBaseEvent("QuitGameButtonPress"));
        });
    }
}
