using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class TimeController : ResetableClassForInit, IService, ISubscribable
{
    #region TimeValues

    private int _maxTime = 30;
    private int _tutorialMaxTime = 15;
    private int _currentMaxTime;

    private int _time;
    public int Time => _time;

    private int _currentMonth = 1;

    public int CurrentMonth => _currentMonth;

    #endregion

    private TimeIndicator _timeIndicator;

    private PeriodController _periodController;
    private PeriodSkipController _gameLifeController;
    private GameController _gameController;
    private SubscribeController _subscribeController;

    private DataSaver _dataSaver;

    public event Action<int> OnTimeChanged;
    public event Action OnPeriodChanged;

    private Action SaveDelegate;

    public TimeController(TimeIndicator timeIndicator) : base () 
    {
        _timeIndicator = timeIndicator;
    }

    public override void Init()
    {
        base.Init();
        _periodController = ServiceLocator.Instance.Get<PeriodController>();
        _gameLifeController = ServiceLocator.Instance.Get<PeriodSkipController>();
        _gameController = ServiceLocator.Instance.Get<GameController>();
        _dataSaver = ServiceLocator.Instance.Get<DataSaver>();

        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        Subscribe();
    }

    private void SetStartValues(int month)
    { 
        if (month == 0)
        {
            SetTutorialValues();
        }
        else
        {
            SetGameValues();
        }
    }

    private void SetGameValues()
    {
        _currentMonth = 1;
        _currentMaxTime = _maxTime;

        _timeIndicator.UpdateMonth(_currentMonth);
        _timeIndicator.Init(_currentMaxTime);
    }

    private void SetTutorialValues()
    {
        _currentMonth = 0;
        _currentMaxTime = _tutorialMaxTime;

        _timeIndicator.UpdateMonth(_currentMonth);
        _timeIndicator.Init(_currentMaxTime);
    }

    public void AddTime(int time)
    {
        var previousTime = _time;

        _time = Mathf.Clamp(_time + time, 0, _currentMaxTime);

        if (PeriodFinished())
        {
            _periodController.FinishPeriod();
        }

        _timeIndicator.SetTime(_time);

        OnTimeChanged?.Invoke(_time - previousTime);
        _gameLifeController.SkipDays();
    }

    public void SetParametersByLoadData(int time, int months)
    {
        if (months == 1) _gameController.GameStarted -= SetGameValues;

        _time = time;
        SetStartValues(months);

        _timeIndicator.SetTime(time);

        if (PeriodFinished())
        {
            _periodController.FinishPeriodWithoutDouble();
            _gameLifeController.SkipDaysWithoutSaving();
        }
    }

    public void UpdateMonth()
    {
        _currentMonth++;
        YandexGame.NewLeaderboardScores("leaderboard", _currentMonth);

        _timeIndicator.UpdateMonth(_currentMonth);
        _time = 0;

        _timeIndicator.SetTime(_time);
        OnPeriodChanged?.Invoke();
    }

    public bool PeriodFinished()
    {
        return _time >= _currentMaxTime;
    }

    public void Subscribe()
    {
        _subscribeController.AddSubscribable(this);
        _gameController.GameStarted += SetGameValues;

        SaveDelegate = () => _dataSaver.SaveMonthsAndTime(CurrentMonth, Time);
        _dataSaver.OnStartSaving += SaveDelegate;
    }

    public void Unsubscribe()
    {
        _gameController.GameStarted -= SetGameValues;
        _dataSaver.OnStartSaving -= SaveDelegate;
    }

    public override void Reset()
    {
        _time = 0;
        _timeIndicator.SetTime(_time);
        SetStartValues(1);
    }
}
