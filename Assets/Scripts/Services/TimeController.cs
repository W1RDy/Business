using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : IService
{
    #region TimeValues

    private int _maxTime = 30;
    private int _time;
    public int Time => _time;

    private int _currentMonth = 1;

    #endregion

    private TimeIndicator _timeIndicator;

    public TimeController(TimeIndicator timeIndicator)
    {
        _timeIndicator = timeIndicator;
        _timeIndicator.Init(_maxTime);
    }

    public void AddTime(int time)
    {
        _time = Mathf.Clamp(_time + time, 0, _maxTime);

        if (_time == _maxTime)
        {
            _currentMonth++;
            _timeIndicator.UpdateMonth(_currentMonth);
            _time = 0;
        }

        _timeIndicator.SetTime(_time);
    }
}
