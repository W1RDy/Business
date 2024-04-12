using CoinsCounter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocatorLoader : MonoBehaviour
{
    [SerializeField] private TimeIndicator _timeIndicator;
    [SerializeField] private CoinsIndicator _handsCoinsIndicator;
    [SerializeField] private CoinsIndicator _bankCoinsIndicator;

    [SerializeField] private RandomController _problemsRandomController;
    [SerializeField] private PeriodSkipController _periodSkipController;
    [SerializeField] private SuggestionsService _suggestionsService;

    [SerializeField] private IconComponentsRandomizer _iconComponentsRandomizer;

    [SerializeField] private AudioDataConfigs _audioDataConfigs;
    [SerializeField] private AudioPlayer _audioPlayerPrefab;
    private AudioPlayer _audioPlayer;

    [SerializeField] private Window[] _windows;

    #region PoolsTransforms

    [SerializeField] private RectTransform _orderPoolContainer;
    [SerializeField] private Transform _orderParent;

    [SerializeField] private RectTransform _goalPoolContainer;
    [SerializeField] private Transform _goalParent;

    [SerializeField] private RectTransform _deliveryOrderPoolContainer;
    [SerializeField] private Transform _deliveryOrderParent;

    [SerializeField] private RectTransform _goodsPoolContainer;
    [SerializeField] private Transform _goodsParent;

    #endregion

    [SerializeField] private CompositeOrder _compositeDeliveryOrder;

    [SerializeField] private GoodsConfig[] _goodsConfigs;
    [SerializeField] private PCConfig[] _pcConfigs;
    [SerializeField] private ProblemConfig[] _problemConfigs;

    private void Awake()
    {
        Bind();
    }

    private void Bind()
    {
        BindAudioServices();

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
        BindOrdersServices();
        BindDeliveryCompositeOrder();

        BindProblemGenerator();
        BindResultsService();

        BindPeriodSkipController();
        BindPeriodController();
        BindTimeController();

        BindPools();

        BindGoodsService();
        BindPCService();

        BindOrderProgressChecker();

        BindButtonService();

        ServiceLocator.Instance.RegisterService();
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
        var gameController = new GameController();
        ServiceLocator.Instance.Register(gameController);
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

    private void BindProblemGenerator()
    {
        var problemGenerator = new ProblemsGenerator(_problemConfigs, _problemsRandomController);
        ServiceLocator.Instance.Register(problemGenerator);
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

    private void BindPCService()
    {
        var pcService = new PCService();
        ServiceLocator.Instance.Register(pcService);

        var pcGenerator = new PCGenerator(_pcConfigs);
        ServiceLocator.Instance.Register(pcGenerator);
    }

    private void BindGoodsService()
    {
        var goodsService = new GoodsService();
        ServiceLocator.Instance.Register(goodsService);

        var goodsGenerator = new GoodsGenerator(_goodsConfigs);
        ServiceLocator.Instance.Register(goodsGenerator);
    }

    private void BindDeliveryCompositeOrder()
    {
        _compositeDeliveryOrder.InitInstance();

        ServiceLocator.Instance.Register(_compositeDeliveryOrder);
    }

    private void BindPools()
    {
        var orderPool = new Pool<Order>(new OrderFactory(_orderPoolContainer), _orderPoolContainer, _orderParent, 2);
        orderPool.Init();

        var goalPool = new Pool<Goal>(new GoalFactory(_goalPoolContainer), _goalPoolContainer, _goalParent, 2);
        goalPool.Init();

        var deliveryOrderPool = new Pool<DeliveryOrder>(new DeliveryOrderFactory(_deliveryOrderPoolContainer), _deliveryOrderPoolContainer, _deliveryOrderParent, 3);
        deliveryOrderPool.Init();

        var goodsPool = new Pool<Goods>(new GoodsFactory(_goodsPoolContainer), _goodsPoolContainer, _goodsParent, 3);
        goodsPool.Init();

        var pcPool = new Pool<PC>(new PCFactory(_goodsPoolContainer), _goodsPoolContainer, _goodsParent, 3);
        pcPool.Init();

        ServiceLocator.Instance.Register(orderPool);
        ServiceLocator.Instance.Register(goalPool);
        ServiceLocator.Instance.Register(deliveryOrderPool);
        ServiceLocator.Instance.Register(goodsPool);
        ServiceLocator.Instance.Register(pcPool);
    }

    private void BindRewardHandler()
    {
        var rewardHandler = new RewardHandler();

        ServiceLocator.Instance.Register(rewardHandler);
    }

    private void BindOrdersServices()
    {
        var ordersService = new OrderService();
        var activeOrderService = new ActiveOrderService();
        var deliveryOrderService = new DeliveryOrderService();
        var rememberedOrderService = new RememberedOrderService();

        ServiceLocator.Instance.Register(ordersService);
        ServiceLocator.Instance.Register(activeOrderService);
        ServiceLocator.Instance.Register(deliveryOrderService);
        ServiceLocator.Instance.Register(rememberedOrderService);
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
        var bankCoinsCounter = new BankCoinsCounter(_bankCoinsIndicator);
        var handCoinsCounter = new HandsCoinsCounter(_handsCoinsIndicator);

        ServiceLocator.Instance.Register(bankCoinsCounter);
        ServiceLocator.Instance.Register(handCoinsCounter);
    }
}
