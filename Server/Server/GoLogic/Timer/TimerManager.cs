namespace GoLogic.Timer;

public class TimerManager
{
    private ISystemTimer blackPlayerTimer;
    private ISystemTimer whitePlayerTimer;
    private StoneColor currentPlayer;

    public TimerManager(ISystemTimer blackTimer, ISystemTimer whiteTimer)
    {
        this.blackPlayerTimer = blackTimer;
        this.whitePlayerTimer = whiteTimer;
    }

    public void SwitchToNextPlayer()
    {
        if (currentPlayer == StoneColor.Black)
        {
            blackPlayerTimer.Pause();
            whitePlayerTimer.Resume();
            currentPlayer = StoneColor.White;
        }
        else
        {
            whitePlayerTimer.Pause();
            blackPlayerTimer.Resume();
            currentPlayer = StoneColor.Black;
        }
    }

    public bool HasTimeRemaining(StoneColor player)
    {
        return player == StoneColor.Black ? blackPlayerTimer.HasTimeRemaining() : whitePlayerTimer.HasTimeRemaining();
    }
}
