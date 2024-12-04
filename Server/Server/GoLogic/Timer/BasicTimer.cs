namespace GoLogic.Timer;

public class BasicTimer : ISystemTimer
{
    private TimeSpan totalTime;
    private DateTime startTime;

    public TimeSpan TotalTime { get => totalTime; }

    public BasicTimer(TimeSpan totalTime)
    {
        this.totalTime = totalTime;
    }

    public void Start()
    {
        startTime = DateTime.Now;
    }

    public void Pause() 
    {
        totalTime -= DateTime.Now - startTime;
    }

    public void Resume() 
    {
        startTime = DateTime.Now;
    }

    public bool HasTimeRemaining()
    {
        return totalTime > TimeSpan.Zero;
    }
}
