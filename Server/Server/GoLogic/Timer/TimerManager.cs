using GoLogic.Goban;

namespace GoLogic.Timer;

/// <summary>
/// Classe qui gère l'exécution des timers
/// </summary>
public class TimerManager
{
    private ISystemTimer blackPlayerTimer;
    private ISystemTimer whitePlayerTimer;
    private StoneColor currentPlayer;

    public TimerManager()
    {
        this.currentPlayer = StoneColor.Black;
        this.blackPlayerTimer = new BasicTimer(TimeSpan.FromMinutes(60));
        this.whitePlayerTimer = new BasicTimer(TimeSpan.FromMinutes(60));
        this.blackPlayerTimer.Start();
    }

    /// <summary>
    /// Change le minuteur pour le joueur suivant.
    /// </summary>
    public void SwitchToNextPlayer()
    {
        if (currentPlayer == StoneColor.Black)
        {
            this.blackPlayerTimer.Pause();
            this.currentPlayer = StoneColor.White;
            this.whitePlayerTimer.Resume();
        }
        else
        {
            whitePlayerTimer.Pause();
            currentPlayer = StoneColor.Black;
            blackPlayerTimer.Resume();
        }
    }

    /// <summary>
    /// Vérifie si le joueur a encore du temps restant.
    /// </summary>
    /// <param name="player">La couleur du joueur.</param>
    /// <returns>Vrai si le joueur a encore du temps, sinon faux.</returns>
    public bool HasTimeRemaining(StoneColor player)
    {
        return player == StoneColor.Black ? blackPlayerTimer.HasTimeRemaining() : whitePlayerTimer.HasTimeRemaining();
    }

    /// <summary>
    /// Obtient le minuteur du joueur précédent.
    /// </summary>
    /// <returns>Le minuteur du joueur précédent.</returns>
    public ISystemTimer GetPreviousTimer()
    {
        return this.currentPlayer == StoneColor.Black ? this.whitePlayerTimer : this.blackPlayerTimer;
    }
}
