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

    [SerializeField] private int _startCoinsInHands;
    [SerializeField] private int _startCoinsInBank;

    public int StartCoinsInHands => _startCoinsInHands;
    public int StartCoinsInBank => _startCoinsInBank;

    private float _oldDifficultyValue;

    [SerializeField] private float[] _problemChances;
    [SerializeField] private float[] _orderChances;

    [SerializeField] private float _ordersRewardValue;
    [SerializeField] private float _ordersTimeValue;
    [SerializeField] private float _problemCostValue;

    [SerializeField] private int _minSkipsBetweenProblems;

    public float[] ProblemChances => _problemChances;
    public float[] OrderChances => _orderChances;

    public float OrdersRewardValue => _ordersRewardValue;
    public float OrdersTimeValue => _ordersTimeValue;
    public float ProblemCostValue => _problemCostValue;

    public int MinSkipsBetweenProblems => _minSkipsBetweenProblems;

    private DifficultyCalculator _difficultyCalculator;

    public event Action DifficultyChanged;

    private void Start()
    {
        _oldDifficultyValue = _difficulty;
        _difficultyCalculator = new DifficultyCalculator(_startCoinsInHands + _startCoinsInBank, 1, _coinsCountWithMaxDifficulty, _monthsCountWithMaxDifficulty);
        _difficultyCalculator.DifficultyCanBeChanged += ChangeDifficultyDelegate;
        StartCoroutine(WaitWhileSceneLoaded());
    }

    private IEnumerator WaitWhileSceneLoaded()
    {
        yield return new WaitForSeconds(1);
        ChangeDifficultyDelegate();
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
        ChangeMinSkipsBetweenProblems(_difficulty);

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
            _problemChances = new float[3] { 30, 15, 10 };
        }
        else if (difficulty < 0.7f)
        {
            _problemChances = new float[3] { 40, 20, 15 };
        }
        else
        {
            _problemChances = new float[3] { 40, 30, 25 };
        }
    }

    private void ChangeOrdersChances(float difficulty)
    {
        if (difficulty < 0.3f)
        {
            _orderChances = new float[3] { 50, 35, 15 };
        }
        else if (difficulty < 0.7f)
        {
            _orderChances = new float[3] { 30, 45, 25 };
        }
        else
        {
            _orderChances = new float[3] { 15, 40, 45 };
        }
    }

    private void ChangeRewardValue(float difficulty)
    {
        _ordersRewardValue = difficulty + 0.8f;
    }

    private void ChangeTimeValue(float difficulty)
    {
        _ordersTimeValue = 2 / (difficulty + 1);
    }

    private void ChangeProblemsCostValue(float difficulty)
    {
        _problemCostValue = (difficulty * 2.4f) + 1;
    }

    private void ChangeMinSkipsBetweenProblems(float difficulty)
    {

        if (difficulty < 0.4f)
        {
            _minSkipsBetweenProblems = 3;
        }
        else if (difficulty < 0.6f)
        {
            _minSkipsBetweenProblems = 2;
        }
        else _minSkipsBetweenProblems = 1;
    }

    private void OnDestroy()
    {
        _difficultyCalculator.DifficultyCanBeChanged -= ChangeDifficultyDelegate;
    }
}

public class DifficultyCalculator : ClassForInitialization, ISubscribable
{
    private int _maxDifficultyWeight;
    private int _minDifficultyWeight;

    private float _coinsCountWeight = 0.2f;
    private float _monthsCountWeight = 0.8f;

    private TimeController _timeController;

    private HandsCoinsCounter _handCoinsCounter;
    private BankCoinsCounter _bankCoinsCounter;

    private SubscribeController _subscribeController;

    public event Action DifficultyCanBeChanged;

    public DifficultyCalculator(int startCoins, int startMonths, int coinsCountWithMaxDifficulty, int monthsCountWithMaxDifficulty) : base()
    {
        _maxDifficultyWeight = CalculateDifficultyWeight(coinsCountWithMaxDifficulty, monthsCountWithMaxDifficulty);
        _minDifficultyWeight = CalculateDifficultyWeight(startCoins, startMonths);
    }

    public override void Init()
    {
        _timeController = ServiceLocator.Instance.Get<TimeController>();

        _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        Subscribe();
    }

    public float CalculateDifficulty()
    {
        var month = _timeController.CurrentMonth;
        var coins = _handCoinsCounter.Coins + _bankCoinsCounter.Coins;

        var currentDifficultyWeight = CalculateDifficultyWeight(coins, month);

        return Mathf.InverseLerp(_minDifficultyWeight, _maxDifficultyWeight, currentDifficultyWeight);
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
