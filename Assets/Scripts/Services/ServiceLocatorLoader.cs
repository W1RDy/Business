using CoinsCounter;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ServiceLocatorLoader : MonoBehaviour
{
    [SerializeField] private PeriodSkipController _periodSkipController;
    [SerializeField] private SuggestionsService _suggestionsService;
    [SerializeField] private ObjectsInitializator _objectsInitializator;

    [SerializeField] private ServiceLocatorEntitiesLoader _servicesLocatorEntitiesLoader;
    [SerializeField] private ServiceLocatorUILoader _servicesLocatorUILoader;

    [SerializeField] private DifficultyController _difficultyController;
    [SerializeField] private SubscribeController _subscribeController;

    [SerializeField] private TutorialController _tutorialController;
    [SerializeField] private TutorialSegment[] _tutorialSegments;

    [SerializeField] private GameController _gameController;
    [SerializeField] private DataSaver _dataSaver;

    private void Awake()
    {
        Bind();
    }

    private void Bind()
    {
        BindInitializator();
        BindDataControllers();

        BindLocalizationInitializer();
        BindTutorial();

        BindSubscribeController();
        BindDifficultyController();

        BindGameController();

        BindSuggestionControllers();
        BindRewardHandler();

        BindButtonChangeController();
        BindConditionChecker();
        BindOrderCompleteHandler();

        _servicesLocatorEntitiesLoader.BindEntities();

        BindResultsService();

        BindPeriodSkipController();
        BindPeriodController();;

        BindOrderProgressChecker();
        _servicesLocatorUILoader.BindUI();

        ServiceLocator.Instance.RegisterService();
    }

    private void BindLocalizationInitializer()
    {
        new LocalizationInitializer();
    }

    private void BindDataControllers()
    {
        var dataLoader = new DataLoader();

        ServiceLocator.Instance.Register(_dataSaver);
        ServiceLocator.Instance.Register(dataLoader);
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

    private void BindDifficultyController()
    {
        ServiceLocator.Instance.Register(_difficultyController);
    }

    private void BindSubscribeController()
    {
        ServiceLocator.Instance.Register(_subscribeController);
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
}