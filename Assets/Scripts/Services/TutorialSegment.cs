using UnityEngine;

public class TutorialSegment : MonoBehaviour, ITutorial
{
    [SerializeField] private TutorialSegmentType _segmentType;
    [SerializeField] private TutorialSegmentPart[] _tutorialSegmentParts;

    public TutorialSegmentType SegmentType => _segmentType;
    public bool IsActivated { get; private set; }

    private int _currentSegmentIndex;

    public void Activate()
    {
        IsActivated = true;
        _currentSegmentIndex = 0;
        ActivatePart();
    }

    public void Deactivate()
    {
        IsActivated = false;
    }

    public bool ConditionCompleted()
    {
        if (GetPart(_currentSegmentIndex).transform.name.StartsWith("Confirm")) Debug.Log(GetPart(_currentSegmentIndex).ConditionCompleted());
        return GetPart(_currentSegmentIndex).ConditionCompleted();
    }

    public void ActivateNextPart()
    {
        DeactivatePart();
        _currentSegmentIndex++;
        if (_currentSegmentIndex < _tutorialSegmentParts.Length) ActivatePart();
    }

    public bool IsNextPartEmpty()
    {
        return _currentSegmentIndex + 1 == _tutorialSegmentParts.Length;
    }

    private void ActivatePart()
    {
        GetPart(_currentSegmentIndex).Activate();
        Debug.Log(_tutorialSegmentParts[_currentSegmentIndex].transform.name);
    }

    private void DeactivatePart()
    {
        GetPart(_currentSegmentIndex).Deactivate();
    }

    private TutorialSegmentPart GetPart(int index)
    {
        return _tutorialSegmentParts[index];
    }
}
