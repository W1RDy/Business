using CoinsCounter;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using YG;

public class GameController : ResetableObjForInit, IService, ISubscribable
{
    public event Action GameFinished;
    public event Action GameStarted;

    public event Action TutorialStarted;
    public event Action TutorialLevelStarted;

    private ResultsActivator _resultsActivator;
    private TutorialActivator _tutorialActivator;
    private GamesConditionChecker _conditionsChecker;

    private HandsCoinsCounter _handsCoinsCounter;
    private BankCoinsCounter _bankCoinsCounter;

    private SubscribeController _subscribeController;
    private LoadSceneController _loadSceneController;

    private DataLoader _dataLoader;

    [SerializeField] private bool _isResetSaves;

    public bool IsFinished { get; private set; }
    public bool IsStartingTutorial { get; private set; }
    public bool IsTutorial { get; set; }

    public override void Init()
    {
        base.Init();
        _resultsActivator = ServiceLocator.Instance.Get<ResultsActivator>();
        _tutorialActivator = ServiceLocator.Instance.Get<TutorialActivator>();
        _conditionsChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        _dataLoader = ServiceLocator.Instance.Get<DataLoader>();

        _handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        _loadSceneController = ServiceLocator.Instance.Get<LoadSceneController>();

        if ( _isResetSaves ) YandexGame.ResetSaveProgress();
        Subscribe();
    }

    public void StartDelegate()
    {
        if (!_conditionsChecker.IsPeriodFinished()) _loadSceneController.LoadScene();
        if (YandexGame.savesData.tutorialPartsCompleted == 0) StartTutorial();
        else if (YandexGame.savesData.tutorialPartsCompleted == 1) StartTutorialLevel();
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
  
    public void RestartGame()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.savesData.tutorialPartsCompleted = 2;
        _loadSceneController.Reset();
    }

    public void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        _dataLoader.OnDataLoaded += StartDelegate;
        _tutorialActivator.TutorialDeactivated += StartDelegate;

        _handsCoinsCounter.CoinsChanged += TryFinishGame;
        _bankCoinsCounter.CoinsChanged += TryFinishGame;

        _loadSceneController.IsReseted += StartDelegate;
    }

    public void Unsubscribe()
    {
        _dataLoader.OnDataLoaded -= StartDelegate;
        _tutorialActivator.TutorialDeactivated -= StartDelegate;

        _handsCoinsCounter.CoinsChanged -= TryFinishGame;
        _bankCoinsCounter.CoinsChanged -= TryFinishGame;

        _loadSceneController.IsReseted -= StartDelegate;
    }

    public override void Reset()
    {
        IsFinished = false;
    }
}