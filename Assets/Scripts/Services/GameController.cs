using System;

public class GameController : IService
{
    public event Action GameFinished;
    public event Action GameStarted;
    public event Action GameRestarted;

    public void FinishGame()
    {
        GameFinished?.Invoke();
    }

    public void StartGame()
    {
        GameStarted?.Invoke();
    }
    
    public void RestartGame()
    {
        GameRestarted?.Invoke();
    }
}