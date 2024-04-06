using System.Data;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SkipTimeSuggestion", menuName = "Suggestions/New Skip Time Suggestion")]
public class SkipTimeSuggestion : Suggestion
{
    private int _time;
    public int Time => _time;

    private TimeController _timeController;

    public void SetSuggestionParameters(int time)
    {
        _time = time;
        if (_timeController == null) _timeController = ServiceLocator.Instance.Get<TimeController>(); 
    }

    public override void ApplySuggestion()
    {
        base.ApplySuggestion();
        _timeController.AddTime(_time);
    }
}