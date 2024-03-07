using System.Linq;
using System.Threading.Tasks;
using ArcanepadSDK.Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (ViewManager.players.All(player => player.isReady))
        {
            connectedPlayersText.text = "Starting game...";

            ViewManager.RefreshUI(UIState.InGame);
        }
    }
}
