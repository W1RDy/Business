using CoinsCounter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour, IService
{
    [SerializeField, Range(0, 1)] private float _difficulty;
    [SerializeField] private int _coinsCountWithMaxDifficulty;
    [SerializeField] private int _monthsCountWithMaxDifficulty;


    private float _oldDifficultyValue;

    [SerializeField] private float[] _problemChances;
    [SerializeField] private float[] _orderChances;

    [SerializeField] private float _ordersRewardValue;
    [SerializeField] private float _ordersTimeValue;
    [SerializeField] private float _problemCostValue;

    public float[] ProblemChances => _problemChances;
    public float[] OrderChances => _orderChances;

    public float OrdersRewardValue => _ordersRewardValue;
    public float OrdersTimeValue => _ordersTimeValue;
    public float ProblemCostValue => _problemCostValue;

    private DifficultyCalculator _difficultyCalculator;

    public event Action DifficultyChanged;

    private void Start()
    {
        _oldDifficultyValue = _difficulty;
        _difficultyCalculator = new DifficultyCalculator(_coinsCountWithMaxDifficulty, _monthsCountWithMaxDifficulty);
        _difficultyCalculator.DifficultyCanBeChanged += ChangeDifficultyDelegate;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (_difficulty != _oldDifficultyValue)
        {
            _oldDifficultyValue = _difficulty;
            ChangeDifficultyParametres(_difficulty);
        }
    }
#endif

    public void ChangeDifficultyParametres(float difficulty)
    {
        _difficulty = Mathf.Clamp(difficulty, 0, 1);

        ChangeProblemChances(_difficulty);
        ChangeOrdersChances(_difficulty);

        ChangeRewardValue(_difficulty);
        ChangeTimeValue(_difficulty);
        ChangeProblemsCostValue(_difficulty);

        DifficultyChanged?.Invoke();
    }

    private void ChangeDifficultyDelegate()
    {
        var difficulty = _difficultyCalculator.CalculateDifficulty();
        ChangeDifficultyParametres(difficulty);
    }

    private void ChangeProblemChances(float difficulty)
    {
        if (difficulty < 0.3f)
        {
            _problemChances = new float[3] { 20, 10, 5 };
        }
        else if (difficulty < 0.7f)
        {
            _problemChances = new float[3] { 30, 20, 15 };
        }
        else
        {
            _problemChances = new float[3] { 40, 25, 20 };
        }
    }

    private void ChangeOrdersChances(float difficulty)
    {
        if (difficulty < 0.3f)
        {
            _orderChances = new float[3] { 50, 30, 20 };
        }
        else if (difficulty < 0.7f)
        {
            _orderChances = new float[3] { 30, 40, 30 };
        }
        else
        {
            _orderChances = new float[3] { 10, 45, 45 };
        }
    }

    private void ChangeRewardValue(float difficulty)
    {
        _ordersRewardValue = 1 * (difficulty + 1);
    }

    private void ChangeTimeValue(float difficulty)
    {
        _ordersTimeValue = 1 * (difficulty + 1);
    }

    private void ChangeProblemsCostValue(float difficulty)
    {
        _problemCostValue = 1 * (difficulty + 1);
    }

    private void OnDestroy()
    {
        _difficultyCalculator.DifficultyCanBeChanged -= ChangeDifficultyDelegate;
    }
}

public class DifficultyCalculator : ISubscribable
{
    private int _maxDifficultyWeight;

    private float _coinsCountWeight = 0.2f;
    private float _monthsCountWeight = 0.8f;

    private TimeController _timeController;

    private HandsCoinsCounter _handCoinsCounter;
    private BankCoinsCounter _bankCoinsCounter;

    private SubscribeController _subscribeController;

    private Action InitDelegate;
    public event Action DifficultyCanBeChanged;

    public DifficultyCalculator(int coinsCountWithMaxDifficulty, int monthsCountWithMaxDifficulty)
    {
        _maxDifficultyWeight = CalculateDifficultyWeight(coinsCountWithMaxDifficulty, monthsCountWithMaxDifficulty);
        Debug.Log(_maxDifficultyWeight);

        InitDelegate = () =>
        {
            _timeController = ServiceLocator.Instance.Get<TimeController>();

            _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
            _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

            _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
            Subscribe();

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };

        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public float CalculateDifficulty()
    {
        var month = _timeController.CurrentMonth;
        var coins = _handCoinsCounter.Coins + _bankCoinsCounter.Coins;

        var currentDifficultyWeight = CalculateDifficultyWeight(coins, month);
        Debug.Log(currentDifficultyWeight);

        return (float)currentDifficultyWeight / _maxDifficultyWeight;
    }

    private int CalculateDifficultyWeight(int coinsCount, int monthsCount)
    {
        return (int)Mathf.Ceil(coinsCount * _coinsCountWeight + monthsCount * _monthsCountWeight);
    }

    private void ActivateEvent()
    {
        DifficultyCanBeChanged?.Invoke();
    }

    public void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        _timeController.OnPeriodChanged += ActivateEvent;
        _handCoinsCounter.CoinsChanged += ActivateEvent;
        _bankCoinsCounter.CoinsChanged += ActivateEvent;
    }

    public void Unsubscribe()
    {
        _timeController.OnPeriodChanged -= ActivateEvent;
        _handCoinsCounter.CoinsChanged -= ActivateEvent;
        _bankCoinsCounter.CoinsChanged -= ActivateEvent;
    }
}
