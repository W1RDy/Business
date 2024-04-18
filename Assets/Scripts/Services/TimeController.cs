using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : ClassForInitialization, IService, ISubscribable
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

    public event Action<int> OnTimeChanged;
    public event Action OnPeriodChanged;

    public TimeController(TimeIndicator timeIndicator) : base () 
    {
        _timeIndicator = timeIndicator;
    }

    public override void Init()
    {
        _periodController = ServiceLocator.Instance.Get<PeriodController>();
        _gameLifeController = ServiceLocator.Instance.Get<PeriodSkipController>();
        _gameController = ServiceLocator.Instance.Get<GameController>();

        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        Subscribe();
    }

    private void SetStartValues()
    {
        if (_gameController.IsTutorial)
        {
            _currentMonth = 0;
            _currentMaxTime = _tutorialMaxTime;
        }
        else
        {
            _currentMonth = 1;
            _currentMaxTime = _maxTime;
        }
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

    public void UpdateMonth()
    {
        _currentMonth++;
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
        _gameController.TutorialStarted += SetStartValues;
        _gameController.GameStarted += SetStartValues;
    }

    public void Unsubscribe()
    {
        _gameController.TutorialStarted -= SetStartValues;
        _gameController.GameStarted -= SetStartValues;
    }
}
