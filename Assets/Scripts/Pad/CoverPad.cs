using ArcanepadSDK.Models;
using UnityEngine;
using UnityEngine.UI;

public class CoverPad : MonoBehaviour
{
    public Button StartButton;

    void Start()
    {
        StartButton = transform.Find("StartButton").GetComponent<Button>();
        StartButton.onClick.AddListener(() =>
        {
            Arcane.Msg.EmitToViews(new ArcaneBaseEvent("CoverStartButtonPress"));
        });
    }
}
