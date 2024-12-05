namespace GoLogic.Timer;

/// <summary>
/// Interfaces pour les différents types de Timer
/// </summary>
public interface ISystemTimer
{
    /// <summary>
    /// Récupère le temps total initial
    /// </summary>
    TimeSpan TotalTime { get; }

    /// <summary>
    /// Lance l'écoulement du timer
    /// </summary>
    void Start();

    /// <summary>
    /// Met en pause l'écoulement du timer
    /// </summary>
    void Pause();

    /// <summary>
    /// Relance le décompte du timer
    /// </summary>
    void Resume();

    /// <summary>
    /// Renvoie s'il reste du temps au timer
    /// </summary>
    /// <returns>True s'il reste du temps False sinon</returns>
    bool HasTimeRemaining();
}
