using CoinsCounter;
using System;
using UnityEngine;

public class GameController : IService
{
    public event Action GameFinished;
    public event Action GameStarted;
    public event Action GameRestarted;

    private ResultsActivator _resultsActivator;
    private GamesConditionChecker _conditionsChecker;

    public bool IsFinished { get; private set; }

    private Action InitDelegate;

    public GameController()
    {
        InitDelegate = () =>
        {
            _resultsActivator = ServiceLocator.Instance.Get<ResultsActivator>();
            _conditionsChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

            var handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
            var bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

            handsCoinsCounter.CoinsChanged += TryFinishGame;
            bankCoinsCounter.CoinsChanged += TryFinishGame;

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    private void TryFinishGame()
    {
        if (_conditionsChecker.IsEnoughCoinsForMinCoins()) FinishGame();
    }

    public void FinishGame()
    {
        IsFinished = true;
        GameFinished?.Invoke();
        _resultsActivator.ActivateResultsOfTheGame();
    }

    public void StartGame()
    {
        GameStarted?.Invoke();
    }
    
    public void RestartGame()
    {
        Debug.Log("Restart");
        GameRestarted?.Invoke();
    }
}