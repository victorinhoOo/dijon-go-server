namespace GoLogic.Timer;

public interface ISystemTimer
{
    void Start();
    void Pause();
    void Resume();
    bool HasTimeRemaining();
}
