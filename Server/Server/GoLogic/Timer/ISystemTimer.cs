namespace GoLogic.Timer;

public interface ISystemTimer
{
    TimeSpan TotalTime { get; }
    void Start();
    void Pause();
    void Resume();
    bool HasTimeRemaining();
}
