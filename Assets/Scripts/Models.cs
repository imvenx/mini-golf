using ArcanepadSDK;
using ArcanepadSDK.Models;

public class RefreshGameStateEvent : ArcaneBaseEvent
{
    public GameState gameState;
    public RefreshGameStateEvent(GameState gameState) : base("RefreshGameState")
    {
        this.gameState = gameState;
    }
}

public class RefreshUIStateEvent : ArcaneBaseEvent
{
    public UIState uiState;
    public RefreshUIStateEvent(UIState uiState) : base("RefreshUIState")
    {
        this.uiState = uiState;
    }
}

public class GameState
{
    public UIState uiState = UIState.GameCover;
}

public class Global
{
    public static GameState gameState = new GameState();
}

public enum UIState
{
    GameCover, SelectCourse, Loading, PlayerDisconnected
}

public class GameEvent
{
    public static string RefreshGameState = "RefreshGameState";
    public static string RefreshUIState = "RefreshUIState";

}

public class Player
{
    public ArcanePad pad;
    public bool isReady = false;
    public Player(ArcanePad pad)
    {
        this.pad = pad;

    }
}