using System.Threading.Tasks;
using ArcanepadSDK.Models;
using TMPro;
using UnityEngine;

public class SelectCourse : MonoBehaviour
{
    public TextMeshProUGUI connectedPlayersText;
    void OnEnable()
    {
        RefreshConectedPlayersText();
    }

    public void RefreshConectedPlayersText()
    {
        string connectedPlayers = "Connected Players: ";

        foreach (var player in ViewManager.players)
        {
            connectedPlayers += player.pad.User.name + (player.isReady ? " ready" : " waiting") + "\n";
        }

        connectedPlayersText.text = connectedPlayers;
    }
}
