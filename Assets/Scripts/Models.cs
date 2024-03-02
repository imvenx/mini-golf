using ArcanepadSDK.Models;

public class RefreshGameState : ArcaneBaseEvent
{
    public int damage;
    public RefreshGameState(int damage) : base("TakeDamage")
    {
        this.damage = damage;
    }
}

public class GameState
{
    public UIState uiState = UIState.GameCover;
}

public enum UIState
{
    GameCover, SelectCourse, Loading, PlayerDisconnected
}