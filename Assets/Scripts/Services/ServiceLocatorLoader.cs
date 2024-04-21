using CoinsCounter;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ServiceLocatorLoader : MonoBehaviour
{
    [SerializeField] private TimeIndicator _timeIndicator;
    [SerializeField] private CoinsIndicator _handsCoinsIndicator;
    [SerializeField] private CoinsIndicator _bankCoinsIndicator;
    [SerializeField] private CoinsChangeView _handsCoinsChangeView;
    [SerializeField] private CoinsChangeView _bankCoinsChangeView;

    [SerializeField] private PeriodSkipController _periodSkipController;
    [SerializeField] private SuggestionsService _suggestionsService;
    [SerializeField] private ObjectsInitializator _objectsInitializator;
    [SerializeField] private ServiceLocatorEntitiesLoader _servicesLocatorEntitiesLoader;

    [SerializeField] private ClicksBlocker _clicksBlocker;

    [SerializeField] private DifficultyController _difficultyController;
    [SerializeField] private SubscribeController _subscribeController;

    [SerializeField] private IconComponentsRandomizer _iconComponentsRandomizer;

    [SerializeField] private AudioDataConfigs _audioDataConfigs;
    [SerializeField] private AudioPlayer _audioPlayerPrefab;
    private AudioPlayer _audioPlayer;

    [SerializeField] private Window[] _windows;

    [SerializeField] private Notification _ordersNotification;
    [SerializeField] private Notification _deliveryOrdersNotification;

    [SerializeField] private TutorialController _tutorialController;
    [SerializeField] private TutorialSegment[] _tutorialSegments;

    [SerializeField] private GameController _gameController;

    private void Awake()
    {
        Bind();
    }

    private void Bind()
    {
        BindInitializator();
        BindTutorial();

        BindSubscribeController();
        BindDifficultyController();

        BindAudioServices();
        BindNotificationServices();

        BindClicksBlocker();
        BindWindowService();
        BindWindowActivator();

        BindGameController();
        
        BindSuggestionControllers();

        BindCoinsCounters();
        BindRewardHandler();

        BindButtonChangeController();
        BindConditionChecker();

        BindIconComponentsRandomizer();

        BindOrderCompleteHandler();
        _servicesLocatorEntitiesLoader.BindEntities();

        BindResultsService();

        BindPeriodSkipController();
        BindPeriodController();
        BindTimeController();

        BindOrderProgressChecker();

        BindButtonService();

        ServiceLocator.Instance.RegisterService();
    }

    private void BindTutorial()
    {
        var tutorialService = new TutorialService(_tutorialSegments);
        var tutorialButtonService = new ButtonsForTutorialService();

        var tutorialButtonActivator = new ButtonForTutorialActivator(tutorialButtonService);

        ServiceLocator.Instance.Register(tutorialButtonService);
        ServiceLocator.Instance.Register(tutorialButtonActivator);

        var tutorialActivator = new TutorialActivator(tutorialService, _tutorialController);
        ServiceLocator.Instance.Register(tutorialActivator);
    }

    private void BindInitializator()
    {
        ServiceLocator.Instance.Register(_objectsInitializator);
    }

    private void BindClicksBlocker()
    {
        ServiceLocator.Instance.Register(_clicksBlocker);
    }

    private void BindDifficultyController()
    {
        ServiceLocator.Instance.Register(_difficultyController);
    }

    private void BindSubscribeController()
    {
        ServiceLocator.Instance.Register(_subscribeController);
    }

    private void BindNotificationServices()
    {
        var notificationService = new NotificationService(new Notification[] {_ordersNotification, _deliveryOrdersNotification});

        var notificationController = new NotificationController(notificationService);
        ServiceLocator.Instance.Register(notificationController);
    }

    private void BindAudioServices()
    {
        var audioService = new AudioService(_audioDataConfigs);

        _audioPlayer = Instantiate(_audioPlayerPrefab);
        _audioPlayer.Init(audioService);

        ServiceLocator.Instance.Register(_audioPlayer);
    }

    private void BindIconComponentsRandomizer()
    {
        ServiceLocator.Instance.Register(_iconComponentsRandomizer);
    }

    private void BindOrderCompleteHandler()
    {
        var orderCompleteHandler = new OrderCompleteHandler();
        ServiceLocator.Instance.Register(orderCompleteHandler);
    }

    private void BindConditionChecker()
    {
        var conditionChecker = new GamesConditionChecker();
        ServiceLocator.Instance.Register(conditionChecker);
    }

    private void BindButtonChangeController()
    {
        var buttonChangeController = new ButtonChangeController();
        ServiceLocator.Instance.Register(buttonChangeController);
    }

    private void BindGameController()
    {
        ServiceLocator.Instance.Register(_gameController);
    }

    private void BindSuggestionControllers()
    {
        ServiceLocator.Instance.Register(_suggestionsService);

        var suggestionGenerator = new SuggestionGenerator();
        ServiceLocator.Instance.Register(suggestionGenerator);
    }

    private void BindPeriodSkipController()
    {
        ServiceLocator.Instance.Register(_periodSkipController);
    }

    private void BindResultsService()
    {
        var resultService = new ResultsOfTheMonthService();
        resultService.ActivateNewResults();

        ServiceLocator.Instance.Register(resultService);

        var resultsActivator = new ResultsActivator();

        ServiceLocator.Instance.Register(resultsActivator);
    }

    private void BindOrderProgressChecker()
    {
        var orderProgressChecker = new OrderProgressChecker();
        ServiceLocator.Instance.Register(orderProgressChecker);
    }

    private void BindRewardHandler()
    {
        var rewardHandler = new RewardHandler();

        ServiceLocator.Instance.Register(rewardHandler);
    }

    private void BindPeriodController()
    {
        var periodController = new PeriodController();
        ServiceLocator.Instance.Register(periodController);
    }

    private void BindWindowService()
    {
        var windowService = new WindowService(_windows);
        ServiceLocator.Instance.Register(windowService);
    }

    private void BindWindowActivator()
    {
        var windowActivator = new WindowActivator();
        ServiceLocator.Instance.Register(windowActivator);
    }

    private void BindButtonService()
    {
        var buttonService = new ButtonService();
        ServiceLocator.Instance.Register(buttonService);
    }

    private void BindTimeController()
    {
        var timeController = new TimeController(_timeIndicator);
        ServiceLocator.Instance.Register(timeController);
    }

    private void BindCoinsCounters()
    {
        var bankCoinsCounter = new BankCoinsCounter(_bankCoinsIndicator, _bankCoinsChangeView);
        var handCoinsCounter = new HandsCoinsCounter(_handsCoinsIndicator, _handsCoinsChangeView);

        ServiceLocator.Instance.Register(bankCoinsCounter);
        ServiceLocator.Instance.Register(handCoinsCounter);
    }
}