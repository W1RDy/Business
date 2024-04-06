using System.Data;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SkipTimeSuggestion", menuName = "Suggestions/New Skip Time Suggestion")]
public class SkipTimeSuggestion : Suggestion, IEventWithTimeParameters
{
    private int _time;
    public int TimeRequirements => _time;

    private TimeController _timeController;

    public void SetParameters(int time)
    {
        _time = time;
        if (_timeController == null) _timeController = ServiceLocator.Instance.Get<TimeController>(); 
    }

    public override void Apply()
    {
        base.Apply();
        _timeController.AddTime(_time);
    }
}