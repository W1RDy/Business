using CoinsCounter;
using System;
using UnityEngine;

public class GameController : ClassForInitialization, IService
{
    public event Action GameFinished;
    public event Action GameStarted;
    public event Action GameRestarted;

    private ResultsActivator _resultsActivator;
    private GamesConditionChecker _conditionsChecker;

    public bool IsFinished { get; private set; }

    public GameController() : base() { }

    public override void Init()
    {
        _resultsActivator = ServiceLocator.Instance.Get<ResultsActivator>();
        _conditionsChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        var handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        var bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        handsCoinsCounter.CoinsChanged += TryFinishGame;
        bankCoinsCounter.CoinsChanged += TryFinishGame;
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