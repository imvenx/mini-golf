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
            Debug.Log("asdasd");
            Arcane.Msg.EmitToViews(new ArcaneBaseEvent("CoverStartButtonPress"));
        });
    }
}
