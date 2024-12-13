namespace GoLogic.Timer;

/// <summary>
/// Version standard d'un timer
/// temps initial qui s'écoule jusqu'à épuisement
/// </summary>
public class BasicTimer : ISystemTimer
{
    private TimeSpan totalTime;
    private DateTime startTime;

    /// <inheritdoc/>
    public TimeSpan TotalTime { get => totalTime; }

    public BasicTimer(TimeSpan totalTime)
    {
        this.totalTime = totalTime;
    }

    /// <inheritdoc/>
    public void Start()
    {
        startTime = DateTime.Now;
    }

    /// <inheritdoc/>
    public void Pause() 
    {
        totalTime -= DateTime.Now - startTime;
    }

    /// <inheritdoc/>
    public void Resume() 
    {
        startTime = DateTime.Now;
    }

    /// <inheritdoc/>
    public bool HasTimeRemaining()
    {
        return totalTime > TimeSpan.Zero;
    }
}
