using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonService : IService
{
    private TimeController _timeController;

    public ButtonService() 
    {
        _timeController = ServiceLocator.Instance.Get<TimeController>();
    }

    public void AddTime(int time)
    {
        _timeController.AddTime(time);
    }
}
