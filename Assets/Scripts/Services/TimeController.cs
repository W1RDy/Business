using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : IService
{
    #region TimeValues

    private int _maxTime = 30;
    private int _time;
    public int Time => _time;

    private int _currentMonth = 1;

    public int CurrentMonth => _currentMonth;

    #endregion

    private TimeIndicator _timeIndicator;

    private PeriodController _periodController;
    private PeriodSkipController _gameLifeController;

    public event Action<int> OnTimeChanged;
    public event Action OnPeriodChanged;

    public TimeController(TimeIndicator timeIndicator)
    {
        _timeIndicator = timeIndicator;
        _timeIndicator.Init(_maxTime);

        _periodController = ServiceLocator.Instance.Get<PeriodController>();
        _gameLifeController = ServiceLocator.Instance.Get<PeriodSkipController>();
    }

    public void AddTime(int time)
    {
        var previousTime = _time;

        _time = Mathf.Clamp(_time + time, 0, _maxTime);

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
        return _time >= _maxTime;
    }
}
