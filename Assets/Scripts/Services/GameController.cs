using CoinsCounter;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : ObjectForInitialization, IService
{
    public event Action GameFinished;
    public event Action GameStarted;
    public event Action GameRestarted;

    public event Action TutorialStarted;
    public event Action TutorialLevelStarted;

    private ResultsActivator _resultsActivator;
    private TutorialActivator _tutorialActivator;
    private GamesConditionChecker _conditionsChecker;

    public bool IsFinished { get; private set; }
    public bool IsStartingTutorial { get; private set; }
    public bool IsTutorial { get; private set; }

    public override void Init()
    {
        _resultsActivator = ServiceLocator.Instance.Get<ResultsActivator>();
        _tutorialActivator = ServiceLocator.Instance.Get<TutorialActivator>();
        _conditionsChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        IsTutorial = true;
        _tutorialActivator.TutorialDeactivated += ChangeGameStateFromTutorial;

        var handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        var bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        handsCoinsCounter.CoinsChanged += TryFinishGame;
        bankCoinsCounter.CoinsChanged += TryFinishGame;

        StartCoroutine(WaitBeforeStart());
    }

    private IEnumerator WaitBeforeStart()
    {
        yield return new WaitForSeconds(0.2f);
        if (IsTutorial) StartTutorial();
        else StartGame();
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
        IsTutorial = false;
        GameStarted?.Invoke();
    }

    public void StartTutorial()
    {
        IsStartingTutorial = true;
        _tutorialActivator.ActivateTutorial(TutorialSegmentType.CompleteOrder);
        TutorialStarted?.Invoke();
    }

    public void StartTutorialLevel()
    {
        IsStartingTutorial = false;
        TutorialLevelStarted?.Invoke();
    }

    private void ChangeGameStateFromTutorial()
    {
        if (IsStartingTutorial) StartTutorialLevel();
        else StartGame();
    }
    
    public void RestartGame()
    {
        Debug.Log("Restart");
        GameRestarted?.Invoke();
    }
}